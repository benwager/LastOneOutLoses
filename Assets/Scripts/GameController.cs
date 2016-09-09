namespace Wager.Ben.LastOneOutLoses
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameController : MonoBehaviour
    {
        #region Enums & Structs
        public enum State
        {
            MENU,
            INSTRUCTIONS,
            PLAY,
            WAITING,
            GAME_OVER,
            CPU_PLAY
        }

        public enum Mode
        {
            SINGLE_PLAYER,
            MULTI_PLAYER
        }

        public enum PlayerType
        {
            HUMAN,
            CPU
        }

        public struct Player
        {
            public PlayerType type;
            public int lastNumber;
            public string name;
        }
        #endregion

        // Easy access from other scripts
        public static GameController Instance;

        public State state;

        public Mode GameMode { get; private set; }

        public List<Player> players;

        public int startingNumber = 101;

        public int maxTurn = 7;

        public int currentTurn = 0;

        public int currentTotal = 0;

        public int lastNumber = 0;

        public int nextNumber = 0;

        public int currentPlayer;

        public GameObject zombiesPrefab;

        private GameObject zombiesInstance;

        System.Random random = new System.Random();

        

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            state = State.MENU;

        }

        public void NewGame(Mode mode)
        {
            if (zombiesInstance != null)
            {
                Destroy(zombiesInstance);
            }

            zombiesInstance = Instantiate(zombiesPrefab);

            startingNumber = FindObjectsOfType<Zombie>().Length;


            GameMode = mode;
            currentTotal = startingNumber;
            lastNumber = 0;
            nextNumber = 0;
            currentPlayer = 0;

            players = new List<Player>();

            Player p1 = new Player();
            p1.lastNumber = 0;
            p1.type = PlayerType.HUMAN;
            p1.name = "P1";
            players.Add(p1);

            Player p2 = new Player();
            p2.lastNumber = 0;
            if(mode == Mode.SINGLE_PLAYER)
            {
                p2.type = PlayerType.CPU;
                p2.name = "CPU";
            }
            else
            {
                p2.type = PlayerType.HUMAN;
                p2.name = "P2";
            }
            players.Add(p2);
        }

        public void StartGame()
        {
            currentPlayer = 0;
            WaitToStartTurn();
        }

        public void WaitToStartTurn()
        {
            state = State.WAITING;

            string msg = players[currentPlayer].name + "'S TURN. CLICK OK WHEN READY";
            HUDController.Instance.DisplayMessage(msg);
            HUDController.Instance.HideDoneButton();
            HUDController.Instance.ShowOKButton();
        }

        public void StartTurn()
        {
            state = State.PLAY;

            currentTurn = 0;
            string msg = "YOU CAN SAVE UP TO " + maxTurn + " ZOMBIE";
            if (maxTurn > 1) msg += "S";
            HUDController.Instance.DisplayMessage(msg);

            HUDController.Instance.HideDoneButton();
            HUDController.Instance.HideOKButton();
            HUDController.Instance.UpdatePlayerText(players[currentPlayer].name);
        }

        public void StartCPUTurn()
        {
            if (state == State.CPU_PLAY) return;

            Debug.Log("Start CPU Turn");
            currentTurn = 0;
            HUDController.Instance.DisplayMessage("CPU's TURN");

            HUDController.Instance.HideDoneButton();
            HUDController.Instance.HideOKButton();
            HUDController.Instance.UpdatePlayerText(players[currentPlayer].name);

            state = State.CPU_PLAY;

            StartCoroutine(WaitToStartCPUTurn());
        }

        private IEnumerator WaitToStartCPUTurn()
        {
            yield return new WaitForSeconds(2);
            int number = GetAINumber();

            Debug.Log("CPU picks " + number);
            for (int i = 0; i < number; i++)
            {
                ZombieController.Instance.PickRandom();
            }

            StopAllCoroutines();
            StartCoroutine(WaitToEndCPUTurn());
        }

        public void EndTurn()
        {
            GetNextPlayer();

            HUDController.Instance.HideDoneButton();
            HUDController.Instance.UpdatePlayerText("");

            state = State.WAITING;

            if(players[currentPlayer].type == PlayerType.CPU)
            {
                StartCPUTurn();
            }
            else
            {
                WaitToStartTurn();
            }
        }

        public void EndGame()
        {
            if (state == State.CPU_PLAY)
            {
                StopAllCoroutines();
                StartCoroutine(WaitForGameOver());
            }
            else
            {
                MenuController.Instance.GotoGameOverView(players[currentPlayer].name);
            }
            state = State.GAME_OVER;
        }

        private IEnumerator WaitForGameOver()
        {
            yield return new WaitForSeconds(2);
            MenuController.Instance.GotoGameOverView(players[currentPlayer].name);
        }

        public bool CanSaveZombie()
        {
            return (state == State.PLAY && currentTurn < maxTurn
                    || state == State.CPU_PLAY && currentTurn < maxTurn);
        }

        public void SaveZombie()
        {
            currentTurn++;

            currentTotal--;

            Debug.Log(currentTotal);

            if(currentTotal <= 1)
            {
                EndGame();
                return;
            }

            if(!CanSaveZombie())
            {
                if(players[currentPlayer].type == PlayerType.CPU)
                {
                    StartCoroutine(WaitToEndCPUTurn());
                    return;
                }
                EndTurn();
                return;
            }

            if (state != State.CPU_PLAY)
            {
                HUDController.Instance.ShowDoneButton();
                string msg = "YOU HAVE SAVED " + currentTurn + " ZOMBIE";
                if (currentTurn > 1) msg += "S";

                HUDController.Instance.DisplayMessage(msg);
            }
        }

        private IEnumerator WaitToEndCPUTurn()
        {
            string msg = "CPU SAVED " + currentTurn + " ZOMBIE";
            if (currentTurn > 1) msg += "S";

            HUDController.Instance.DisplayMessage(msg);

            yield return new WaitForSeconds(2);
            EndTurn();
        }


        /// <summary>
        /// Sets current Player id to the next available player
        /// </summary>
        private void GetNextPlayer()
        {
            if (currentPlayer < players.Count - 1)
            {
                currentPlayer++;
            }
            else
            {
                currentPlayer = 0;
            }


        }

        /// <summary>
        /// Quick check if current number can win
        /// </summary>
        /// <returns>True / False</returns>
        private bool CanPlayerWin()
        {
            return (currentTotal % (maxTurn + 1) >= 1);
        }

        /// <summary>
        /// Gets next number for CPU player
        /// Always attempts to land opponent on a losing number
        /// </summary>
        /// <returns>int</returns>
        private int GetAINumber()
        {
            if (currentTotal <= maxTurn + 1)
            {
                nextNumber = currentTotal - 1;
            }
            else if (currentTotal == maxTurn + 2)
            {
                nextNumber = 1;
            }
            else if (currentTotal > maxTurn + 2 && currentTotal <= maxTurn + 1 + maxTurn + 1)
            {
                nextNumber = currentTotal - (maxTurn + 2);
            }
            else if (CanPlayerWin())
            {
                nextNumber = currentTotal % (maxTurn + 1);
            }
            else
            {
                nextNumber = random.Next(1, maxTurn + 1);
            }

            return nextNumber;
        }
    }
}
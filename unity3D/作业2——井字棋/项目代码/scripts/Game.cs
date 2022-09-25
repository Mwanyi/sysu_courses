using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    // object
    public GUISkin playerSkin; //skin
    public GUIStyle whiteBackground;
    public Texture2D restartLogo;
    public Texture2D singleLogo, doubleLogo;
    public Texture2D o, x, empty;
    public Texture2D o_win, x_win;

    // constant
    private const int gameheight = 450;
    private const int gamewidth = 700;
    private const int chesssize = 100;
    private const int edge = 30;

    // model
    private const int SINGLE = 1;
    private const int DOUBLE = 2;

    //chessboard, players
    private Chessboard chessboard;
    private Player player1;
    private Player player2;
    private ChessAI chessAI;

    private bool is_start;
    private int gamemode;
    private int order; // 0-8
    private int draw; // 平局数

    void Start()
    {
        chessboard = new Chessboard();
        chessboard.CleanChessboard();
        // set player0-O first to drop chess, player1-X
        player1 = new Player(0, true);
        player2 = new Player(1, false);
        chessAI = new ChessAI();
        is_start = true;
        gamemode = SINGLE;
        order = 0;
        draw = 0;
    }

    void OnGUI()
    {
        GUI.skin = playerSkin;
        // set full screen is a box, name is empty
        GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "");
        GUI.BeginGroup(new Rect(Screen.width * 0.5f - 300, Screen.height * 0.5f - 200, gamewidth, gameheight));
            // set chessboard
            GUI.Box(new Rect(edge + 2, edge + 2, 3 * chesssize - 4, 3 * chesssize - 4), "", whiteBackground); // 棋盘
            // AI drop chess
            AIAction();
            // 显示棋子与玩家下棋
            ChessBoardUpdata();
            // 判断胜负
            GameCheak();
            GUI.Label(new Rect(2 * edge + 3 * chesssize, edge, 150, 100), "The Game");
            // 更新比分表
            GradeUpdata();
            GUI.BeginGroup(new Rect(2 * edge + 3 * chesssize, edge + 200, 150, 100));
                //设置重新开启一局
                SetResentButton();
                // 更改游戏模式
                SetGameModeButton();
            GUI.EndGroup();
        GUI.EndGroup();
    }

    // 获得当前局的棋子类型
    private int GetChessStyle()
    {
        int chessstyle;
        // if player1 is first, 则双数为O-0，默认玩家1为O，玩家2为X
        if (player1.GetFirstDrop())
        {
            chessstyle = order % 2;
        }
        else
        {
            chessstyle = (order + 1) % 2;
        }
        return chessstyle;
    }

    // chessAI走棋
    private void AIAction()
    {
        int chessstylethisorder = GetChessStyle();
        // 如果为单人模式并且本轮为player2，即本轮为AI走棋
        if (is_start && gamemode == SINGLE && chessstylethisorder == player2.GetChessType())
        {
            // 找到AI走棋的最好策略
            int[] AI_conditioon = chessAI.ChessAIMakePosition(chessboard, player2, player1, order);
            // 更新本轮的棋盘
            chessboard.updataChessboard(chessstylethisorder, AI_conditioon[0], AI_conditioon[1]);
            // 轮数加1，打印棋盘
            order++;
            print(chessAI.PrintAIChess());
        }

    }

    // 棋盘更新
    private void ChessBoardUpdata()
    {
        // 得到当前的棋盘和当前的棋子类型
        int[,] board = chessboard.GetBoard();
        bool[,] win_board = chessboard.GetWinBoard();
        int chessstylethisorder = GetChessStyle();
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Texture2D texture;
                Texture2D texture_Light;
                if (board[i, j] == player1.GetChessType())
                {
                    texture = o;
                    texture_Light = o_win;
                }
                else if (board[i, j] == player2.GetChessType())
                {
                    texture = x;
                    texture_Light = x_win;
                }
                else
                {
                    texture = empty;
                    texture_Light = empty;
                }
                // 如果本轮已经结束且棋盘当前位置为false（输）
                if (is_start == false && win_board[i, j] == false)
                {
                    // 将当前位置的棋子放置为调暗的棋子
                    GUI.Button(new Rect(edge + j * chesssize + 2, edge + i * chesssize + 2, chesssize - 4, chesssize - 4), texture_Light);
                }
                else
                {
                    if (GUI.Button(new Rect(edge + j*chesssize+2, edge+i*chesssize+2, chesssize-4, chesssize - 4), texture) && is_start)
                    {
                        // 如果模式为双人或者为单人且是玩家方下棋，直接按照点击的位置进行棋盘更新
                        if (gamemode == DOUBLE || gamemode == SINGLE && chessstylethisorder == player1.GetChessType())
                        {
                            if (chessboard.updataChessboard(chessstylethisorder, i, j))
                            {
                                order++;
                            }
                        }
                    }
                }
            }
        }
    }

    // 检查游戏是否结束
    private void GameCheak()
    {
        if (is_start)
        {
            // 找到赢家对应的棋子
            int result = chessboard.winner();
            // 限制当前局面不变
            is_start = false;
            // 如果结果是玩家1获胜，设置玩家1获胜
            if (result == player1.GetChessType())
            {
                player1.SetWinCount();
            }
            else if (result == player2.GetChessType())
            {
                player2.SetWinCount();
            }
            else
            {
                // 如果已经下完9个棋子，平局
                if (order == 9)
                {
                    draw++;
                }
                else
                {
                    is_start = true;
                }
            }
        }
    }

    // 更新分数板
    private void GradeUpdata()
    {
        // 以玩家1赢得场数：平局数：玩家2赢得场数
        string wininfo = player1.GetWinCount().ToString() + " : " + draw.ToString() + " : " + player2.GetWinCount().ToString();
        // 设置为一个label放置在GUI界面
        GUI.Label(new Rect(2 * edge + 3 * chesssize, edge + 100, 150, 100), wininfo);
    }

    // 设置重启按钮
    private void SetResentButton()
    {
        if (GUI.Button(new Rect(10, 30, 40, 40), restartLogo))
        {
            //将棋盘置为空
            order = 0;
            is_start = true;
            chessboard.CleanChessboard();
            // 交换先后手
            player1.ChangeFirstDrop();
            player2.ChangeFirstDrop();
        }
    }

    // 更改游戏模式
    private void SetGameModeButton()
    {
        if (gamemode == SINGLE)
        {
            if (GUI.Button(new Rect(100, 30, 40, 40), singleLogo))
            {
                order = 0;
                is_start = true;
                chessboard.CleanChessboard();
                player1.ChangeFirstDrop();
                player2.ChangeFirstDrop();
                gamemode = DOUBLE;
                player1.ResetWinCount();
                player2.ResetWinCount();
                draw = 0;
            }
        }
        else
        {
            if (GUI.Button(new Rect(100, 30, 40, 40), doubleLogo))
            {
                order = 0;
                is_start = true;
                chessboard.CleanChessboard();
                player1.ChangeFirstDrop();
                player2.ChangeFirstDrop();
                gamemode = SINGLE;
                player1.ResetWinCount();
                player2.ResetWinCount();
                draw = 0;
            }
        }
    }
}
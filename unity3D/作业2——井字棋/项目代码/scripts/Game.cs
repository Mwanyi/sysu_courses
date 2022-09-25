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
    private int draw; // ƽ����

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
            GUI.Box(new Rect(edge + 2, edge + 2, 3 * chesssize - 4, 3 * chesssize - 4), "", whiteBackground); // ����
            // AI drop chess
            AIAction();
            // ��ʾ�������������
            ChessBoardUpdata();
            // �ж�ʤ��
            GameCheak();
            GUI.Label(new Rect(2 * edge + 3 * chesssize, edge, 150, 100), "The Game");
            // ���±ȷֱ�
            GradeUpdata();
            GUI.BeginGroup(new Rect(2 * edge + 3 * chesssize, edge + 200, 150, 100));
                //�������¿���һ��
                SetResentButton();
                // ������Ϸģʽ
                SetGameModeButton();
            GUI.EndGroup();
        GUI.EndGroup();
    }

    // ��õ�ǰ�ֵ���������
    private int GetChessStyle()
    {
        int chessstyle;
        // if player1 is first, ��˫��ΪO-0��Ĭ�����1ΪO�����2ΪX
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

    // chessAI����
    private void AIAction()
    {
        int chessstylethisorder = GetChessStyle();
        // ���Ϊ����ģʽ���ұ���Ϊplayer2��������ΪAI����
        if (is_start && gamemode == SINGLE && chessstylethisorder == player2.GetChessType())
        {
            // �ҵ�AI�������ò���
            int[] AI_conditioon = chessAI.ChessAIMakePosition(chessboard, player2, player1, order);
            // ���±��ֵ�����
            chessboard.updataChessboard(chessstylethisorder, AI_conditioon[0], AI_conditioon[1]);
            // ������1����ӡ����
            order++;
            print(chessAI.PrintAIChess());
        }

    }

    // ���̸���
    private void ChessBoardUpdata()
    {
        // �õ���ǰ�����̺͵�ǰ����������
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
                // ��������Ѿ����������̵�ǰλ��Ϊfalse���䣩
                if (is_start == false && win_board[i, j] == false)
                {
                    // ����ǰλ�õ����ӷ���Ϊ����������
                    GUI.Button(new Rect(edge + j * chesssize + 2, edge + i * chesssize + 2, chesssize - 4, chesssize - 4), texture_Light);
                }
                else
                {
                    if (GUI.Button(new Rect(edge + j*chesssize+2, edge+i*chesssize+2, chesssize-4, chesssize - 4), texture) && is_start)
                    {
                        // ���ģʽΪ˫�˻���Ϊ����������ҷ����壬ֱ�Ӱ��յ����λ�ý������̸���
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

    // �����Ϸ�Ƿ����
    private void GameCheak()
    {
        if (is_start)
        {
            // �ҵ�Ӯ�Ҷ�Ӧ������
            int result = chessboard.winner();
            // ���Ƶ�ǰ���治��
            is_start = false;
            // �����������1��ʤ���������1��ʤ
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
                // ����Ѿ�����9�����ӣ�ƽ��
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

    // ���·�����
    private void GradeUpdata()
    {
        // �����1Ӯ�ó�����ƽ���������2Ӯ�ó���
        string wininfo = player1.GetWinCount().ToString() + " : " + draw.ToString() + " : " + player2.GetWinCount().ToString();
        // ����Ϊһ��label������GUI����
        GUI.Label(new Rect(2 * edge + 3 * chesssize, edge + 100, 150, 100), wininfo);
    }

    // ����������ť
    private void SetResentButton()
    {
        if (GUI.Button(new Rect(10, 30, 40, 40), restartLogo))
        {
            //��������Ϊ��
            order = 0;
            is_start = true;
            chessboard.CleanChessboard();
            // �����Ⱥ���
            player1.ChangeFirstDrop();
            player2.ChangeFirstDrop();
        }
    }

    // ������Ϸģʽ
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
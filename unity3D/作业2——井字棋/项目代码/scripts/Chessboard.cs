using System.Collections;
using UnityEngine;

public class Chessboard {
    private int[,] board = new int[3,3];
    // win_board is used to show the win chess(highlight)
    private bool[,] win_board = new bool[3,3];
    // show the board state
    private const int o = 0;
    private const int x = 1;
    private const int EMPTY = -1;

    public void CleanChessboard() {
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 3; j++) {
                board[i,j] = EMPTY;
                win_board[i,j] = false;
            }
        }
    }

    public int[,] GetBoard()
    {
        return board;
    }

    public bool[,] GetWinBoard()
    {
        return win_board;
    }

    // 更新棋盘，当给与的坐标设置为player
    public bool updataChessboard(int player, int x, int y)
    {
        if (board[x, y] == EMPTY)
        {
            board[x, y] = player;
            return true;
        }
        // 如果不是空，则更新失败
        else
        {
            return false;
        }
    }

    public int winner()
    {
        /* case 1:
        X X X    X . .
        . . . or X . .
        . . .    X . .
        */
        for (int i = 0; i < 3; i++)
        {
            // 如果存在一列的棋子相同，则返回对应棋子
            if (board[0, i] == board[1, i] && board[1, i] == board[2, i])
            {
                if (board[0, i] == o)
                {
                    win_board[0, i] = win_board[1, i] = win_board[2, i];
                    return o;
                }
                else if (board[0, i] == x)
                {
                    win_board[0, i] = win_board[1, i] = win_board[2, i];
                    return x;
                }
            }
            // 如果存在一行的棋子相同，则返回对应棋子
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
            {
                if (board[i, 0] == o)
                {
                    win_board[i, 0] = win_board[i, 1] = win_board[i, 2];
                    return o;
                }
                else if (board[i, 0] == x)
                {
                    win_board[i, 0] = win_board[i, 1] = win_board[i, 2];
                    return x;
                }
            }
        }
        /* case 2:
        X . .    . . X
        . X . or . X .
        . . X    X . .
        */
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2])
        {
            if (board[0, 0] == o)
            {
                win_board[0, 0] = win_board[1, 1] = win_board[2, 2];
                return o;
            }
            else if (board[0, 0] == x)
            {
                win_board[0, 0] = win_board[1, 1] = win_board[2, 2];
                return x;
            }
        }
        else if (board[2, 0] == board[1, 1] && board[1, 1] == board[0, 2])
        {
            if (board[2, 0] == o)
            {
                win_board[2, 0] = win_board[1, 1] = win_board[0, 2];
                return o;
            }
            else if (board[2, 0] == x)
            {
                win_board[2, 0] = win_board[1, 1] = win_board[0, 2];
                return x;
            }
        }
        return EMPTY;
    }
}
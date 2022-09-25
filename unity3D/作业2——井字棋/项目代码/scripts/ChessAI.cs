using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessAI 
{
    // the position AI will drop chess
    private int[,] AI_chess = new int[3, 3];
    public ChessAI()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                AI_chess[i, j] = 0;
            }
        }
    }

    // ȡ�õ�ǰ������AI������߲���ʽ
    public int[] ChessAIMakePosition(Chessboard chessboard, Player chessai, Player player, int order)
    {
        int[,] board = chessboard.GetBoard();
        // �洢AI������±�
        int[] AI_position = new int[2];
        // ���AIΪ��������Ĳ���Ϊ�׾֣���AI���ѡ��һ��λ������
        if (chessai.GetFirstDrop() && order == 0)
        {
            AI_position = this.GetRandomPosition();
        }
        else
        {
            int AIchesstype = chessai.GetChessType();
            int playerchesstype = player.GetChessType();
            AI_position = ChessAIMakePosition2(board, AIchesstype, playerchesstype);
        }
        return AI_position;
    }

    public int[] ChessAIMakePosition2(int[,] chessboard, int AIchesstype, int playerchesstype)
    {
        // ��Ų�ͬλ�õķ���
        int[,] score = new int[3, 3];
        // ��Ų�ͬ����ķ���
        int[] lineweight = new int[8];
        // ������Ϊ3��
        const int score0 = -50;
        const int score1 = 0;
        const int score2 = 60;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                // ������̵�ǰλ��ΪAI������,��ǰλ�÷�����Ϊ���
                if (chessboard[i, j] == AIchesstype)
                {
                    score[i, j] = score2;
                }
                // ���Ϊ������ӣ��������Ϊ���
                else if (chessboard[i, j] == playerchesstype)
                {
                    score[i, j] = score0;
                }
                // ���û�����ӣ��������Ϊ0
                else
                {
                    score[i, j] = score1;
                }
            }
        }

        /*
              0          1          2
            X X X      . . .      . . .
            . . .  OR  X X X  OR  . . .
            . . .      . . .      X X X
        */
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                lineweight[i] += score[i, j];
            }
        }
        /*
              3          4          5
            X . .      . X .      . . X
            X . .  OR  . X .  OR  . . X
            X . .      . X .      . . X
        */
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                lineweight[i + 3] += score[j, i];
            }
        }
        /*
              6
            X . .   
            . X .  
            . . X 
        */
        lineweight[6] = score[0, 0] + score[1, 1] + score[2, 2];
        /*
              7
            . . X   
            . X .  
            X . . 
        */
        lineweight[7] = score[0, 2] + score[1, 1] + score[2, 0];

        // �ڲ�ͬλ���ϵ����ӿ���ʤ����������ֵ��������[0,0]λ�õ����ӿ��������0��3��6
        AI_chess[0, 0] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[6]));
        AI_chess[0, 1] = System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[4]));
        AI_chess[0, 2] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[0]), System.Math.Abs(lineweight[5])), System.Math.Abs(lineweight[7]));

        AI_chess[1, 0] = System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[3]));
        AI_chess[1, 1] = System.Math.Max(System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[4])), System.Math.Abs(lineweight[6])), System.Math.Abs(lineweight[7]));
        AI_chess[1, 2] = System.Math.Max(System.Math.Abs(lineweight[1]), System.Math.Abs(lineweight[5]));

        AI_chess[2, 0] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[7]));
        AI_chess[2, 1] = System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[4]));
        AI_chess[2, 2] = System.Math.Max(System.Math.Max(System.Math.Abs(lineweight[2]), System.Math.Abs(lineweight[3])), System.Math.Abs(lineweight[6]));

        // �����ǰλ���Ѿ������ӣ����������Ϊ0
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (chessboard[i, j] == AIchesstype)
                {
                    AI_chess[i, j] = 0;
                }
                else if (chessboard[i, j] == playerchesstype)
                {
                    AI_chess[i, j] = 0;
                }
            }
        }
        // �ҵ��������ֵ��λ��
        int max = 0, x = 0, y = 0;
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                int flag = Random.Range(0, 2);
                if (flag == 0)
                {
                    if (AI_chess[i, j] > max)
                    {
                        max = AI_chess[i, j];
                        x = i;
                        y = j;
                    }
                }
                else
                {
                    if (AI_chess[i, j] >= max)
                    {
                        max = AI_chess[i, j];
                        x = i;
                        y = j;
                    }
                }
            }
        }
        int[] XY = new int[2];
        XY[0] = x;
        XY[1] = y;
        return XY;
    }

    // ����õ�λ��
    private int[] GetRandomPosition()
    {
        int[] Position = new int[2];
        // �����������
        Position[0] = Random.Range(0, 3);
        Position[1] = Random.Range(0, 3);
        return Position;
    }

    // ��ӡ��ǰ����
    public string PrintAIChess()
    {
        string info = "The AIChess\n";
        for (int i = 0; i< 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                info += AI_chess[i, j].ToString() + " ";
            }
            info += "\n";
        }
        return info;
    }
}
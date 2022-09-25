using System.Collections;
using UnityEngine;

public class Player
{
    private int win_number;
    private int chesstype; //0-O,1-X
    private bool first_drop; // is the first one to drop chess

    public Player(int chesstype, bool first_drop)
    {
        this.win_number = 0;
        this.chesstype = chesstype;
        this.first_drop = first_drop;
    }

    public bool GetFirstDrop() => this.first_drop;

    public int GetChessType()
    {
        return chesstype;
    }

    public void SetChessType(int i)
    {
        chesstype = i;
    }

    public void SetWinCount()
    {
        win_number++;
    }

    public int GetWinCount()
    {
        return win_number;
    }

    public void ResetWinCount()
    {
        win_number = 0;
    }

    public void ChangeFirstDrop()
    {
        this.first_drop = !first_drop;
    }
}
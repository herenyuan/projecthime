using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CallBack(bool _input);
public enum State
{
    none,
    up,
    down,
}
public class CmdTrigger
{
    public State state = State.up;

    public string Name;
    public bool Action = false;
    public Action<CmdTrigger> Call;
    public CmdTrigger(string name, bool value)
    {
        Name = name;
    }
    public void Update(bool _input)
    {
        Action = _input;
        if (Action)
        {
            if (state == State.up)
            {
                state = State.down;
                Call.Invoke(this);
            }
        }
        else
        {
            if (state == State.down)
            {
                state = State.up;
            }
        }
    }
}
public class CmdVector2
{
    public State state = State.none;

    public string Name;
    public Vector2 Value = Vector2.zero;
    public Action<CmdVector2> Call;
    public CmdVector2(string name, Vector2 value)
    {
        Name = name;
        Value = value;
    }
    public void Update(Vector2 direction)
    {
        Value = direction;
        Call.Invoke(this);
    }
}

public class InputManager
{
    public CmdVector2 cmdMove;
    public CmdTrigger cmdJump;
    public CmdTrigger cmdChange;

    private string btnMove;
    private string btnJump;
    private string btnChange;

    public enum InputType
    {
        KEYBOARD_1  = 0,//键盘1
        KEYBOARD_2  = 1,//键盘2
        HANDLE_1    = 2,//手柄1
        HANDLE_2    = 3,//手柄2
    }

    public InputManager(InputType _type)
    {
        switch (_type)
        {
            case InputType.HANDLE_1:
                btnMove         = "Handle1-Move";
                btnJump         = "Handle1-Jump";
                btnChange       = "Handle1-Change";
                break;
            case InputType.HANDLE_2:
                btnMove         = "Handle2-Move";
                btnJump         = "Handle2-Jump";
                btnChange       = "Handle2-Change";
                break;
            case InputType.KEYBOARD_1:
                btnMove         = "Keyboard1-Move";
                btnJump         = "Keyboard1-Jump";
                btnChange       = "Keyboard1-Change";
                break;
            case InputType.KEYBOARD_2:
                btnMove         = "Keyboard2-Move";
                btnJump         = "Keyboard2-Jump";
                btnChange       = "Keyboard2-Change";
                break;
        }
        cmdMove = new CmdVector2("Move", Vector2.zero);
        cmdJump = new CmdTrigger("Jump", false);
        cmdChange = new CmdTrigger("Change", false);
    }

    public void Action()
    {
        float _move = Input.GetAxis(btnMove);
        bool _jump = Input.GetButton(btnJump);
        bool _change = Input.GetButton(btnChange);

        cmdMove.Update(new Vector2(_move, 0));
        cmdJump.Update(_jump);
        cmdChange.Update(_change);
    }

    public static InputType GetTypeByName(string name)
    {
        switch (name)
        {
            case "handle1":
                return InputType.HANDLE_1;
                break;
            case "handle2":
                return InputType.HANDLE_2;
                break;
            case "keyboard1":
                return InputType.KEYBOARD_1;
                break;
            case "keyboard2":
                return InputType.KEYBOARD_2;
                break;
        }
        return InputType.KEYBOARD_1;
    }

}

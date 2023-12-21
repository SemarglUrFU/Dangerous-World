using System;

public interface IInGameMenu
{
    public void Open();
    public void Close();
    public Action<IInGameMenu> OnClose {get; set;}
}

namespace TareEngine
{
    public interface IAdventureGame
    {
        void Init();
        void ClearGraphic();
        void DescribeRoom(bool isLook = false);
        void WhatNext();

        void ClearTerminal();
        void ClearKeyboard();
        void ToggleKeyboard(bool enableKeyboard);
    }
}

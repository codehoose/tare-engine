namespace TareEngine
{
    public interface IAdventureGame
    {
        /// <summary>
        /// Initialize the game.
        /// </summary>
        void Init();
        /// <summary>
        /// Clear the graphic in the current room.
        /// </summary>
        void ClearGraphic();
        /// <summary>
        /// Describe the current room.
        /// </summary>
        /// <param name="isLook"></param>
        void DescribeRoom(bool isLook = false);
        /// <summary>
        /// Display the "What next?" prompt to the player.
        /// </summary>
        void WhatNext();
        /// <summary>
        /// Clear the terminal screen.
        /// </summary>
        void ClearTerminal();
        /// <summary>
        /// Clear the keyboard input buffer.
        /// </summary>
        void ClearKeyboard();
        /// <summary>
        /// Toggle the keyboard input on or off.
        /// </summary>
        /// <param name="enableKeyboard">Set to true to enable keyboard input, false to disable it</param>
        void ToggleKeyboard(bool enableKeyboard);
    }
}

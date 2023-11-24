﻿namespace PacMan.Shared.Patterns.Observer
{
    public interface ISubscriber
    {
        Task Notify(int index, string name);
    }
}
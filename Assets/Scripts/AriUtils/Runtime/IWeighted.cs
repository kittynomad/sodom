/*****************************************************************************
// File Name : IWeighted.cs
// Author : Arcadia Koederitz
// Creation Date : 10/04/2026
// Last Modified : 10/04/2026
//
// Brief Description : Interface for objects that are selected using weighted randomness.
*****************************************************************************/

namespace AriUtils
{
    public interface IWeighted
    {
        int Weight { get; }
    }
}

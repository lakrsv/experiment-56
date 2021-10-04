// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="IPoolable.cs" author="Lars-Kristian Svenoy" company="Cardlike">
// //  Copyright @ 2018 Cardlike. All rights reserved.
// // </copyright>
// // <summary>
// //   TODO - Insert file description
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------

namespace Utilities.ObjectPool
{
    public interface IPoolable
    {
        bool IsEnabled { get; }

        void Activate();

        void Deactivate();
    }
}
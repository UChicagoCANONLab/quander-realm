using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Shared
{
    public static class GameEvents
    {
        public static UnityEvent<Minigame> testEvent = new UnityEvent<Minigame>();

    }
}

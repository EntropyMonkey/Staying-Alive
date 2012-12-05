// Messenger.cs v0.1 (20090925) by Rod Hyde (badlydrawnrod).

//
// This is a C# messenger (notification center) for Unity. It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other.

// prints all events which are fired
//#define DEBUG_EVENTS_ALL

using System;
using System.Collections.Generic;
using UnityEngine;

static public class MessengerEvents
{
	public const string ClearQueuedEvents = "clear events";
}

/**
  * A messenger for events that have no parameters.
  */
static public class Messenger
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static Messenger()
	{
		AddListener(MessengerEvents.ClearQueuedEvents, ClearEvents);
	}

	static public void ClearEvents()
	{
		eventTable.Clear();
	}

	static public void AddListener( string eventType, Callback handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Create an entry for this event type if it doesn't already exist.
			if ( !eventTable.ContainsKey( eventType ) )
			{
				eventTable.Add( eventType, null );
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback)eventTable[eventType] + handler;
		}
	}

	static public void RemoveListener( string eventType, Callback handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Only take action if this event type exists.
			if ( eventTable.ContainsKey( eventType ) )
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if ( eventTable[eventType] == null )
				{
					eventTable.Remove( eventType );
				}
			}
		}
	}

	static public void Invoke( string eventType )
	{
#if DEBUG_EVENTS_ALL
        MessengerDebug.print("Event: " + eventType + "\n" + Environment.StackTrace);
#endif
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if ( eventTable.TryGetValue( eventType, out d ) )
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback callback = (Callback)d;

			// Invoke the delegate if it's not null.
			if ( callback != null )
			{
				callback();
			}
		}
	}
}

/**
  * A messenger for events that have one parameter of type T.
  */

static public class Messenger<T>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static Messenger()
	{
		Messenger.AddListener(MessengerEvents.ClearQueuedEvents, ClearEvents);
	}

	static public void ClearEvents()
	{
		eventTable.Clear();
	}

	static public void AddListener( string eventType, Callback<T> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Create an entry for this event type if it doesn't already exist.
			if ( !eventTable.ContainsKey( eventType ) )
			{
				eventTable.Add( eventType, null );
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T>)eventTable[eventType] + handler;
		}
	}

	static public void RemoveListener( string eventType, Callback<T> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Only take action if this event type exists.
			if ( eventTable.ContainsKey( eventType ) )
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T>)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if ( eventTable[eventType] == null )
				{
					eventTable.Remove( eventType );
				}
			}
		}
	}

	static public void Invoke( string eventType, T arg1 )
	{
#if DEBUG_EVENTS_ALL
        MessengerDebug.print("Event<" + typeof(T).ToString().Replace("+", ".") + ">: " + eventType);
#endif
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if ( eventTable.TryGetValue( eventType, out d ) )
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T> callback = (Callback<T>)d;

			// Invoke the delegate if it's not null.
			if ( callback != null )
			{
				callback( arg1 );
			}
		}
	}
}

/**
  * A messenger for events that have two parameters of types T and U.
  */

static public class Messenger<T, U>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static Messenger()
	{
		Messenger.AddListener(MessengerEvents.ClearQueuedEvents, ClearEvents);
	}

	static public void ClearEvents()
	{
		eventTable.Clear();
	}

	static public void AddListener( string eventType, Callback<T, U> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Create an entry for this event type if it doesn't already exist.
			if ( !eventTable.ContainsKey( eventType ) )
			{
				eventTable.Add( eventType, null );
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T, U>)eventTable[eventType] + handler;
		}
	}

	static public void RemoveListener( string eventType, Callback<T, U> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Only take action if this event type exists.
			if ( eventTable.ContainsKey( eventType ) )
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T, U>)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if ( eventTable[eventType] == null )
				{
					eventTable.Remove( eventType );
				}
			}
		}
	}

	static public void Invoke( string eventType, T arg1, U arg2 )
	{
#if DEBUG_EVENTS_ALL
        MessengerDebug.print("Event<" + typeof(T).ToString().Replace("+", ".") + ", " +
            typeof(U).ToString().Replace("+", ".") + ">: " + eventType);
#endif
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if ( eventTable.TryGetValue( eventType, out d ) )
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T, U> callback = (Callback<T, U>)d;

			// Invoke the delegate if it's not null.
			if ( callback != null )
			{
				callback( arg1, arg2 );
			}
		}
	}
}

/**
* A messenger for events that have two parameters of types T, U, V.
*/

static public class Messenger<T, U, V>
{
	private static Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();

	static Messenger()
	{
		Messenger.AddListener(MessengerEvents.ClearQueuedEvents, ClearEvents);
	}

	static public void ClearEvents()
	{
		eventTable.Clear();
	}

	static public void AddListener( string eventType, Callback<T, U, V> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Create an entry for this event type if it doesn't already exist.
			if ( !eventTable.ContainsKey( eventType ) )
			{
				eventTable.Add( eventType, null );
			}
			// Add the handler to the event.
			eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] + handler;
		}
	}

	static public void RemoveListener( string eventType, Callback<T, U, V> handler )
	{
		// Obtain a lock on the event table to keep this thread-safe.
		lock ( eventTable )
		{
			// Only take action if this event type exists.
			if ( eventTable.ContainsKey( eventType ) )
			{
				// Remove the event handler from this event.
				eventTable[eventType] = (Callback<T, U, V>)eventTable[eventType] - handler;

				// If there's nothing left then remove the event type from the event table.
				if ( eventTable[eventType] == null )
				{
					eventTable.Remove( eventType );
				}
			}
		}
	}

	static public void Invoke( string eventType, T arg1, U arg2, V arg3 )
	{
#if DEBUG_EVENTS_ALL
    MessengerDebug.print("Event<" + typeof(T).ToString().Replace("+", ".") + ", " +
        typeof(U).ToString().Replace("+", ".") + ", " + typeof(V).ToString().Replace("+", ".") + ">: " + eventType);
#endif
		Delegate d;
		// Invoke the delegate only if the event type is in the dictionary.
		if ( eventTable.TryGetValue( eventType, out d ) )
		{
			// Take a local copy to prevent a race condition if another thread
			// were to unsubscribe from this event.
			Callback<T, U, V> callback = (Callback<T, U, V>)d;

			// Invoke the delegate if it's not null.
			if ( callback != null )
			{
				callback( arg1, arg2, arg3 );
			}
		}
	}
}

/* MessengerDebug:MonoBehaviour
* used for debugging because Messenger does not inherit from MonoBehaviour or ScriptableObject
*/

public class MessengerDebug : MonoBehaviour { }
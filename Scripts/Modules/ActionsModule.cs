﻿#if UNITY_EDITOR && UNITY_2017_2_OR_NEWER
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Modules
{
	sealed class ActionsModule : MonoBehaviour, IConnectInterfaces
	{
		public List<ActionMenuData> menuActions { get { return m_MenuActions; } }
		List<ActionMenuData> m_MenuActions = new List<ActionMenuData>();
		List<IAction> m_Actions;

		public void RemoveActions(List<IAction> actions)
		{
			m_MenuActions.Clear();
			m_MenuActions.AddRange(m_MenuActions.Where(a => !actions.Contains(a.action)));
		}

		void Start()
		{
			SpawnActions();
		}

		void SpawnActions()
		{
			IEnumerable<Type> actionTypes = ObjectUtils.GetImplementationsOfInterface(typeof(IAction));
			m_Actions = new List<IAction>();
			foreach (Type actionType in actionTypes)
			{
				// Don't treat vanilla actions or tool actions as first class actions
				if (actionType.IsNested || !typeof(MonoBehaviour).IsAssignableFrom(actionType))
					continue;

				var action = ObjectUtils.AddComponent(actionType, gameObject) as IAction;
				var attribute = (ActionMenuItemAttribute)actionType.GetCustomAttributes(typeof(ActionMenuItemAttribute), false).FirstOrDefault();

				this.ConnectInterfaces(action);

				if (attribute != null)
				{
					var actionMenuData = new ActionMenuData()
					{
						name = attribute.name,
						sectionName = attribute.sectionName,
						priority = attribute.priority,
						action = action,
					};

					m_MenuActions.Add(actionMenuData);
				}

				m_Actions.Add(action);
			}

			m_MenuActions.Sort((x, y) => y.priority.CompareTo(x.priority));
		}
	}
}
#endif

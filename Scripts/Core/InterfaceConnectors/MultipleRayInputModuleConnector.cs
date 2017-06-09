﻿#if UNITY_EDITOR && UNITY_2017_2_OR_NEWER
using UnityEditor.Experimental.EditorVR.Modules;

namespace UnityEditor.Experimental.EditorVR.Core
{
	partial class EditorVR
	{
		class MultipleRayInputModuleConnector : Nested, ILateBindInterfaceMethods<MultipleRayInputModule>
		{
			public void LateBindInterfaceMethods(MultipleRayInputModule provider)
			{
				IIsHoveringOverUIMethods.isHoveringOverUI = provider.IsHoveringOverUI;
			}
		}
	}
}
#endif

#pragma once
#ifndef __WRAPPER__
#define __WRAPPER__

#include "PluginSettings.h"
#include "Vector2D.h"

#ifdef __cplusplus
extern "C"
{
#endif
	
	PLUGIN_API int GetID();

	PLUGIN_API void SetID(const int id);

	PLUGIN_API Vector2D GetPosition();

	PLUGIN_API void SetPosition(const float x, const float y);

#ifdef __cplusplus
}
#endif

#endif

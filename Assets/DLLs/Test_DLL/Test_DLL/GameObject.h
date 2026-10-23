#pragma once
#ifndef __GAME_OBJECT__
#define __GAME_OBJECT__

#include "PluginSettings.h"
#include "Vector2D.h"

class PLUGIN_API GameObject
{
public:
	GameObject();

	int GetID() const;
	void SetID(int id = 0);

	Vector2D GetPosition() const;
	void SetPosition(float x = 0.0f, float y = 0.0f);
private:
	int m_id;
	Vector2D m_position;
};

#endif
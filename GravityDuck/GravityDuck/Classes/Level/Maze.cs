using System;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Graphics;

using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;

namespace GravityDuck
{
	//Our Maze class V2 by @AW && @AS
	public class Maze
	{
		private TextureInfo groundBlock1;
		private TextureInfo groundBlock2;
		
		private TextureInfo ceilingBlock1;
		private TextureInfo ceilingBlock2;
		
		private TextureInfo treeBlock;
		
		private int coinCount;
		private Coin[] coins;
		
		private int gemCount;
		private Gem[] gems;
		
		private int spikeCount;
		private Spikes[] spikes;
		
		private int windTunnelCount;
		private WindTunnel[] windTunnels;
		
		private int blackHoleCount;
		private BlackHole[] blackHoles;
		
		private int laserGateCount;
		private LaserGate[] laserGates;
				
		private int breakableWallCount;
		private BreakableWall[] breakableWalls;
		
		
		private LevelFlag levelFlag;
		private bool levelFinished;
		
		private SpriteUV[,] 	sprites; //Each block is a sprite
		int[,] mazeLevel;
		private int mazeWidth, mazeHeight; //Width and height for the maze
						
		public Maze (Scene scene)
		{
			levelFinished = false;
			
			//Maze height and width
			mazeWidth = 8;
			mazeHeight = 11;
			coinCount = 20; 
			gemCount = 3;
			spikeCount = 5;
			windTunnelCount = 0;
			blackHoleCount = 0;
			laserGateCount = 0;
			breakableWallCount = 0;
			
			//Load in the textures here
			//Ground Block Textures
			groundBlock1 = new TextureInfo("/Application/textures/Level/groundBlock1.png");
			groundBlock2 = new TextureInfo("/Application/textures/Level/groundBlock2.png");
			
			//Ceiling Block Textures
			ceilingBlock1 = new TextureInfo("/Application/textures/Level/ceilingBlock1.png");
			ceilingBlock2 = new TextureInfo("/Application/textures/Level/ceilingBlock2.png");
			
			treeBlock = new TextureInfo("/Application/textures/Level/treeBlock.png");
			
			sprites	= new SpriteUV[mazeWidth, mazeHeight]; //Initalise the sprites
						
			int[,] mazeLevel1 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
						     	  {5, 1, 0, 3, 1, 0, 0, 0, 0, 3, 5},
							      {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							  	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							 	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							 	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							  	  {5, 2, 0, 0, 0, 0, 3, 1, 0, 4, 5},
							  	  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
			
			//Initislise and position spikes
			spikes = new Spikes[spikeCount];
			
			
			spikes[0] = new Spikes(scene, 2);
			spikes[1] = new Spikes(scene, 1);
			spikes[2] = new Spikes(scene, 1);
			spikes[3] = new Spikes(scene, 2);
			spikes[4] = new Spikes(scene, 1);
			
			spikes[0].setPosition(new Vector2(500.0f, 215.0f));
			spikes[1].setPosition(new Vector2(690.0f, 690.0f));
			spikes[1].getSprite().Rotate(-1.5707963268f);
			spikes[2].setPosition(new Vector2(360.0f, 620.0f));
			spikes[3].setPosition(new Vector2(50.0f, 1075.0f));
			spikes[3].getSprite().Rotate(-1.5707963268f);
			spikes[4].setPosition(new Vector2(270.0f, 1050.0f));
			
			// Initialise and position wind tunnels	RMDS
			if(windTunnelCount > 0)
			{
				windTunnels = new WindTunnel[windTunnelCount];
				
				windTunnels[0] = new WindTunnel(scene, WindTunnel.Direction.LEFT);
				windTunnels[0].setPosition(new Vector2(800.0f, 340.0f));
			}
			
			//	Initialise and position black holes		RMDS
			if(blackHoleCount > 0)
			{
				blackHoles = new BlackHole[blackHoleCount];
				
				blackHoles[0] = new BlackHole(scene, BlackHole.Direction.UP);
				blackHoles[0].setPosition(new Vector2(800.0f, 280.0f));
			}
			
			//	Initialise and position laser gates		RMDS
			if(laserGateCount > 0)
			{
				laserGates = new LaserGate[laserGateCount];
				
				laserGates[0] = new LaserGate(scene, LaserGate.Direction.UP);
				laserGates[0].setPosition(new Vector2(300.0f, 270.0f));	
			}
			
			//	Initialise and position breakable walls		RMDS
			if(breakableWallCount > 0)
			{
				breakableWalls = new BreakableWall[breakableWallCount];
				
				breakableWalls[0] = new BreakableWall(scene, BreakableWall.Direction.UP, 60.0f);
				breakableWalls[0].setPosition(new Vector2(300.0f, 270.0f));
			}

			
			
			//Initialise maze tiles
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (mazeLevel1[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock1);
						sprites[i,j].Quad.S   = groundBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 2) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock2);
						sprites[i,j].Quad.S   = groundBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 3) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock1);
						sprites[i,j].Quad.S   = ceilingBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 4) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock2);
						sprites[i,j].Quad.S   = ceilingBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel1[i,j] == 5) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(treeBlock);
						sprites[i,j].Quad.S   = treeBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else
					{
						sprites[i,j] = new SpriteUV(null);
					}
				}
			}
			
			//Initialise and position coins
			coins = new Coin[coinCount];
			
			for (int i = 0; i <= coinCount - 1; i++)
			{
				coins[i] = new Coin(scene);
			}
			
			Vector2 position = new Vector2(200.0f, 1000.0f);
			
			coins[0].setPosition(new Vector2(325.0f, 280.0f));
			coins[1].setPosition(new Vector2(375.0f, 280.0f));
			coins[2].setPosition(new Vector2(425.0f, 280.0f));
			coins[3].setPosition(new Vector2(600.0f, 400.0f));
			coins[4].setPosition(new Vector2(600.0f, 400.0f));
			coins[5].setPosition(new Vector2(650.0f, 400.0f));
			coins[6].setPosition(new Vector2(700.0f, 400.0f));
			coins[7].setPosition(new Vector2(650.0f, 710.0f));
			coins[8].setPosition(new Vector2(600.0f, 760.0f));
			coins[9].setPosition(new Vector2(550.0f, 810.0f));
			coins[10].setPosition(new Vector2(325.0f, 710.0f));
			coins[11].setPosition(new Vector2(275.0f, 710.0f));
			coins[12].setPosition(new Vector2(225.0f, 710.0f));
			coins[13].setPosition(new Vector2(140.0f, 1100.0f));
			coins[14].setPosition(new Vector2(140.0f, 1150.0f));
			coins[15].setPosition(new Vector2(140.0f, 1200.0f));
			coins[16].setPosition(new Vector2(700.0f, 1150.0f));
			coins[17].setPosition(new Vector2(650.0f, 1150.0f));
			coins[18].setPosition(new Vector2(600.0f, 1150.0f));
			coins[19].setPosition(new Vector2(550.0f, 1150.0f));
			
			//Initislise and position gems
			gems = new Gem[gemCount];
			
			for(int i = 0; i <= gemCount - 1; i++)
			{
				gems[i] = new Gem(scene);
			}
			
			gems[0].setPosition(new Vector2(850.0f, 550.0f));
			gems[1].setPosition(new Vector2(220.0f, 900.0f));
			gems[2].setPosition(new Vector2(325.0f, 1250.0f));
			
			//Initialise and position Level Flag
			levelFlag = new LevelFlag(scene);
			
			levelFlag.setPosition(new Vector2(820.0f, 1130.0f));
		}
		
		public void LoadLevel(Scene scene, int levelNum)
		{
			scene.RemoveAllChildren(true);			
			
			if(levelNum == 1)
			{
				int[,] mazeLevel1 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
						     	  	  {5, 1, 0, 3, 1, 0, 0, 0, 0, 3, 5},
							      	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							  	      {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							 	  	  {5, 2, 0, 4, 2, 0, 3, 1, 0, 4, 5},
							 	 	  {5, 1, 0, 3, 1, 0, 4, 2, 0, 3, 5},
							  	  	  {5, 2, 0, 0, 0, 0, 3, 1, 0, 4, 5},
							  	  	  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
				
				mazeLevel = mazeLevel1;
				mazeWidth = 8;
				mazeHeight = 11;
				sprites	= new SpriteUV[mazeWidth, mazeHeight];
			}
			else if(levelNum == 2)
			{
				int[,] mazeLevel2 = { {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5},
					                  {5, 1, 0, 3, 1, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 5, 5, 0, 1, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 3, 1, 0, 3, 1, 0, 2, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 4, 2, 0, 0, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 3, 1, 0, 5, 5, 5, 5, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 4, 2, 0, 0, 0, 4, 2, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 1, 0, 5, 5, 5, 5, 0, 3, 1, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 2, 0, 0, 0, 3, 1, 0, 4, 2, 0, 4, 0, 0, 0, 0, 0, 5},
									  {5, 5, 5, 5, 0, 4, 2, 0, 3, 1, 0, 3, 0, 0, 0, 0, 0, 5},
									  {5, 0, 0, 1, 0, 3, 1, 0, 4, 2, 0, 5, 5, 5, 5, 5, 5, 5},
									  {5, 0, 0, 2, 0, 5, 5, 0, 3, 1, 0, 0, 0, 0, 0, 0, 0, 5},
									  {5, 0, 0, 1, 0, 0, 0, 0, 4, 2, 5, 5, 5, 5, 5, 5, 5, 5},
									  {5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5} };
				
				mazeLevel = mazeLevel2;
				mazeWidth = 14;
				mazeHeight = 18;
				sprites	= new SpriteUV[mazeWidth, mazeHeight];
			}
			
			for (int i = 0; i < mazeWidth; ++i) //Basic tile engine
			{
    			for (int j = 0; j < mazeHeight; ++j) //Goes through every number/coordinate in the array
				{
					if (mazeLevel[i,j] == 1) //If the tile has an ID of 1 it's a rigid block
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock1);
						sprites[i,j].Quad.S   = groundBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 2) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(groundBlock2);
						sprites[i,j].Quad.S   = groundBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 3) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock1);
						sprites[i,j].Quad.S   = ceilingBlock1.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 4) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(ceilingBlock2);
						sprites[i,j].Quad.S   = ceilingBlock2.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else if(mazeLevel[i,j] == 5) //Else place the block off-screen
					{
						sprites[i,j] 		  = new SpriteUV(treeBlock);
						sprites[i,j].Quad.S   = treeBlock.TextureSizef;
						sprites[i,j].Position = new Vector2(128.0f*i, 142.0f*j); //Place them at intervals
						scene.AddChild(sprites[i,j]);
					}
					else
					{
						sprites[i,j] = new SpriteUV(null);
					}
				}
			}
		}
		
		public void Dispose() //Dispose texture data
		{
			groundBlock1.Dispose();
			groundBlock2.Dispose();
			ceilingBlock1.Dispose();
			ceilingBlock2.Dispose();
			treeBlock.Dispose();
		}
		
		public bool HasCollidedWithPlayer(Bounds2 player) //Check if the a sprite has hit a part of the maze
		{
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
				   return true;
				}
			}
			return false;
	
		}
		
		public bool HasHitSide(Bounds2 player, int gravity) //Checks if the player has hit the side of the maze and not the floor
		{
			foreach(SpriteUV spri in sprites)
			{
				Bounds2 mazeTile = spri.GetlContentLocalBounds();
				spri.GetContentWorldBounds(ref mazeTile); //Get all of the maze bounds
				if (player.Overlaps(mazeTile)) //Return true if the player overlaps with the maze
				{
					if (checkSides(player, spri, gravity)) //Return true if the player has come into contact with a side
					{
					   return true;
					}
				}
			}
			return false;
		}
		
		public bool checkSides(Bounds2 player, SpriteUV sprite2, int gravity) //ComparrightRotation = false;es 2 sprites to see is the left or right side has intersected
		{
			Bounds2 mazeTile = sprite2.GetlContentLocalBounds();
			sprite2.GetContentWorldBounds(ref mazeTile); 
			if (gravity == 1) //Down
			{
				if (((player.Point01.X < mazeTile.Point11.X) || (player.Point11.X > mazeTile.Point01.X)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point01.Y) < mazeTile.Point01.Y) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 2) // Right
			{
				if (((player.Point10.Y < mazeTile.Point11.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point10.X) > mazeTile.Point11.X) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 3) // Up
			{
				//if (((player.Point10.X < mazeTile.Point00.X) || (player.Point00.X > mazeTile.Point10.X)))
				if (((player.Point01.X < mazeTile.Point11.X) || (player.Point11.X > mazeTile.Point01.X)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point10.Y) > mazeTile.Point10.Y) //If the tile is above the player
						return true;
					else
						return false;
				}
				else
						return false;
			}
			else if (gravity == 4) // Left
			{
				if (((player.Point10.Y < mazeTile.Point11.Y) || (player.Point10.Y > mazeTile.Point11.Y)))
				{ //If the left side of the player is past the right side of the tile and vica versa
					if ((player.Point11.X) < mazeTile.Point11.X) //If the tile is above the player
						return true;	
					else
						return false;
				}
				else
						return false;
			}
			else
				return false;
				
			}
		
		//Check Collectable Collisions
		public int CheckCollectableCollision(SpriteUV sprite, Scene scene)
		{
			foreach(Coin coin in coins)
			{
				bool collide = coin.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
					int scoreValue = coin.Collected(scene);
					return scoreValue;
				}
			}
			
			foreach(Gem gem in gems)
			{
				bool collide = gem.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
					int scoreValue = gem.Collected(scene);
					return scoreValue;
				}
			}
			//If no collisions occur
			return 0;
		}
		
		//Check Obstacle Collisions
		public bool CheckObstacleCollisions(SpriteUV sprite)
		{
			foreach(Spikes spike in spikes)
			{
				bool collide = spike.HasCollidedWithPlayer(sprite);
				
				if (collide)
				{
				   return true;
				}
			}
			return false;
		}
		
		//Check Level Flag Collisions
		public bool CheckFlagCollision(SpriteUV sprite)
		{
			bool collide = levelFlag.HasCollidedWithPlayer(sprite);
				
			if (collide)
			{
				return true;
			}
			return false;
		}
		
		//Returns Level Complete Boolean
		public bool IsLevelComplete()
		{
			return levelFinished;
		}
		
		//Set Level Complete Boolean
		public void SetLevelFinished(bool newLevelFinished)
		{
			levelFinished = newLevelFinished;
		}
		
		// Check collision between player and the wind force exerted by the Wind Tunnels	RMDS
		public Vector2 CheckWindTunnel(Player player)
		{
			Vector2 force = new Vector2(0.0f, 0.0f);
			
			if(windTunnels != null)
			{
				for(int i = 0; i < windTunnelCount; i++)
					if(windTunnels[0].CheckPlayerPos(player))
						force = windTunnels[0].CalculateForce(player);		
			}
		
			return force;
		}
		
		// Check collision between player and Black Hole gravitational pulls	RMDS
		public Vector2 CheckBlackHole(Player player)
		{
			Vector2 force = new Vector2(0.0f, 0.0f);
			
			if(blackHoles != null)
			{
				for(int i = 0; i < blackHoleCount; i++)
					if(blackHoles[0].CheckPlayerPos(player))
						force = blackHoles[0].CalculateForce(player);
			}
			
			return force;
		}
		
		// Check collision between player and Laser Gates	RMDS
		public bool CheckLaserGates(Player player)
		{
			if(laserGates != null)
			{
				for(int i = 0; i < laserGateCount; i++)
					if(laserGates[0].CheckPlayerPos(player))
						return true;
			}
			
			return false;
		}
		
		public bool CheckBreakableWalls(Player player)
		{
			if(breakableWalls != null)
			{
				for(int i = 0; i < breakableWallCount; i++)
					if(breakableWalls[i].HasCollidedWithPlayer(player.Sprite))	
						return breakableWalls[i].CheckIfBreak(player.GetMomentum());
			}
			
			return false;
		}	
	}
}

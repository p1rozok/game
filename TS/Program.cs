using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Program
{
    static RenderWindow window;
    static float playerX = 100;
    static float playerY = 220;
    static float playerSpeed = 400;
    static int playerDirection = -1;
    static Clock clock = new Clock();
    static int moneySize = 32;
    static int moneyX;
    static int moneyY;
    static int playerSizeX = 64;
    static int playerSizeY = 128;
    static int playerScore = 0;
    static int highScore = 0;
    static bool isLose = false;
    static bool isPaused = false;
    static Font font;
    static Text scoreText;
    static Music backgroundMusic;
    static Sound failSound;
    static Sound takeMoneySound;
    static Texture backgroundTexture;
    static Sprite backgroundSprite;
    static Texture playerTexture1;
    static Sprite playerSprite1;
    static Texture playerTexture2;
    static Sprite playerSprite2;
    static Sprite currentPlayerSprite;
    static Texture[] moneyTextures = new Texture[3];
    static Sprite moneySprite;
    static Texture muteTexture;
    static Sprite muteSprite;
    static Texture gameOverTexture;
    static Sprite gameOverSprite;
    static Texture menuTexture;
    static Sprite menuSprite;
    static Texture startTexture;
    static Sprite startSprite;
    static Texture exitTexture;
    static Sprite exitSprite;
    static Texture pauseIconTexture;
    static Sprite pauseIconSprite;
    static Texture letterMTexture;
    static Sprite letterMSprite;
    static Texture letterRTexture;
    static Sprite letterRSprite;
    static bool isMusicPlaying = true;
    static bool isSoundOn = true;

    static void LoadMoneyTextures()
    {
        moneyTextures[0] = new Texture("1Money.png");
        moneyTextures[1] = new Texture("2Money.png");
        moneyTextures[2] = new Texture("3Money.png");
    }

    static void ResetGame()
    {
        playerX = 100;
        playerY = 220;
        playerDirection = -1;
        playerSpeed = 400;
        playerScore = 0;
        Random rnd = new Random();
        moneyX = rnd.Next(20, 1920 - moneySize - 20);
        moneyY = rnd.Next(20, 1080 - moneySize - 20);
        moneySprite.Texture = moneyTextures[rnd.Next(0, 3)]; // Инициализация с случайной текстурой монетки
        isLose = false;
    }

    static void PlayerMove(float deltaTime, uint windowWidth, uint windowHeight)
    {
        if (Keyboard.IsKeyPressed(Keyboard.Key.W)) playerDirection = 0;
        if (Keyboard.IsKeyPressed(Keyboard.Key.D)) playerDirection = 1;
        if (Keyboard.IsKeyPressed(Keyboard.Key.S)) playerDirection = 2;
        if (Keyboard.IsKeyPressed(Keyboard.Key.A)) playerDirection = 3;
        if (Keyboard.IsKeyPressed(Keyboard.Key.R))
        {
            ResetGame();
        }

        if (playerDirection == 0 && playerY > 0) playerY -= playerSpeed * deltaTime;
        if (playerDirection == 1 && playerX < windowWidth - playerSizeX) playerX += playerSpeed * deltaTime;
        if (playerDirection == 2 && playerY < windowHeight - playerSizeY) playerY += playerSpeed * deltaTime;
        if (playerDirection == 3 && playerX > 0) playerX -= playerSpeed * deltaTime;

        if (playerX < 0 || playerX > windowWidth - playerSizeX || playerY < 0 || playerY > windowHeight - playerSizeY)
        {
            isLose = true;
            if (playerScore > highScore)
            {
                highScore = playerScore;
            }
        }
    }

    static void ToggleSound()
    {
        if (isSoundOn)
        {
            backgroundMusic.Volume = 0;
            takeMoneySound.Volume = 0;
            failSound.Volume = 0;
        }
        else
        {
            backgroundMusic.Volume = 100;
            takeMoneySound.Volume = 100;
            failSound.Volume = 100;
        }
        isSoundOn = !isSoundOn;
    }

    static void DrawPlayer()
    {
        currentPlayerSprite.Position = new Vector2f(playerX, playerY);
        window.Draw(currentPlayerSprite);
    }

    static void DrawMoney()
    {
        moneySprite.Position = new Vector2f(moneyX, moneyY);
        window.Draw(moneySprite);
    }

    static void DrawScore()
    {
        scoreText.DisplayedString = $"Score: {playerScore}/{highScore}";
        scoreText.Position = new Vector2f(150, 38);
        window.Draw(scoreText);
    }

    static void DrawMenu()
    {
        menuSprite.Position = new Vector2f((1920 - menuSprite.GetGlobalBounds().Width) / 2, (1080 - menuSprite.GetGlobalBounds().Height) / 2 - 150);
        window.Draw(menuSprite);

        startSprite.Position = new Vector2f(menuSprite.Position.X, menuSprite.Position.Y + 150);
        window.Draw(startSprite);

        exitSprite.Position = new Vector2f(menuSprite.Position.X, startSprite.Position.Y + 150);
        window.Draw(exitSprite);
    }

    static void DrawMuteIcon()
    {
        muteSprite.Position = new Vector2f(1920 - muteSprite.GetGlobalBounds().Width - 20, 1080 - muteSprite.GetGlobalBounds().Height - 20);
        window.Draw(muteSprite);
    }

    static void DrawPauseIcon()
    {
        pauseIconSprite.Position = new Vector2f(20, 1080 - pauseIconSprite.GetGlobalBounds().Height - 20);
        window.Draw(pauseIconSprite);
    }

    static void DrawLetterMIcon()
    {
        letterMSprite.Position = new Vector2f(1920 - letterMSprite.GetGlobalBounds().Width - 20, 1080 - letterMSprite.GetGlobalBounds().Height - 200);
        window.Draw(letterMSprite);
    }

    static void DrawLetterRIcon()
    {
        letterRSprite.Position = new Vector2f(1920 - letterRSprite.GetGlobalBounds().Width - 30, 1080 - letterRSprite.GetGlobalBounds().Height - 320);
        window.Draw(letterRSprite);
    }

    static void Main(string[] args)
    {
        window = new RenderWindow(new VideoMode(1920, 1080), "Game1");
        window.Closed += (sender, e) => ((RenderWindow)sender).Close();

        font = new Font("arial.ttf");

        scoreText = new Text("Score: 0/0", font, 40)
        {
            FillColor = Color.Black
        };

        backgroundMusic = new Music("bg_music.wav");
        backgroundMusic.Loop = true;
        backgroundMusic.Play();

        failSound = new Sound(new SoundBuffer("Fail.wav"));

        takeMoneySound = new Sound(new SoundBuffer("takeMoney.wav"));

        backgroundTexture = new Texture("background.png");
        backgroundSprite = new Sprite(backgroundTexture);

        playerTexture2 = new Texture("character.png");
        playerSprite2 = new Sprite(playerTexture2);

        playerTexture1 = new Texture("leftpng.png");
        playerSprite1 = new Sprite(playerTexture1);

        currentPlayerSprite = playerSprite1;

        LoadMoneyTextures();
        Random rnd = new Random();
        moneySprite = new Sprite(moneyTextures[rnd.Next(0, 3)]);
        ResetGame();

        muteTexture = new Texture("Mute.png");
        muteSprite = new Sprite(muteTexture);

        gameOverTexture = new Texture("GameOver.png");
        gameOverSprite = new Sprite(gameOverTexture)
        {
            Position = new Vector2f((1920 - gameOverTexture.Size.X) / 2, (1080 - gameOverTexture.Size.Y) / 2)
        };

        menuTexture = new Texture("Menu.png");
        menuSprite = new Sprite(menuTexture);
        startTexture = new Texture("Start.png");
        startSprite = new Sprite(startTexture);
        exitTexture = new Texture("Exit.png");
        exitSprite = new Sprite(exitTexture);
        pauseIconTexture = new Texture("WASD.png");
        pauseIconSprite = new Sprite(pauseIconTexture);

        letterMTexture = new Texture("M.png");
        letterMSprite = new Sprite(letterMTexture);

        letterRTexture = new Texture("R.png");
        letterRSprite = new Sprite(letterRTexture);

        bool wasEscapePressed = false;

        while (window.IsOpen)
        {
            window.DispatchEvents();
            uint windowWidth = window.Size.X;
            uint windowHeight = window.Size.Y;
            float deltaTime = clock.Restart().AsSeconds();

            if (!isLose && !isPaused)
            {
                PlayerMove(deltaTime, windowWidth, windowHeight);

                if (isLose)
                {
                    if (isSoundOn) failSound.Play();
                }

                if (playerX + playerSizeX > moneyX && playerX < moneyX + moneySize && playerY + playerSizeY > moneyY && playerY < moneyY + moneySize)
                {
                    moneyX = rnd.Next(20, 1920 - moneySize - 20);
                    moneyY = rnd.Next(20, 1080 - moneySize - 20);
                    moneySprite.Texture = moneyTextures[rnd.Next(0, 3)]; // Смена текстуры монетки при подборе

                    playerScore += 1;
                    playerSpeed += 10;

                    if (isSoundOn) takeMoneySound.Play();
                }
            }
            else if (isLose)
            {
                window.Draw(gameOverSprite);
                if (Keyboard.IsKeyPressed(Keyboard.Key.W) || Keyboard.IsKeyPressed(Keyboard.Key.A) || Keyboard.IsKeyPressed(Keyboard.Key.S) || Keyboard.IsKeyPressed(Keyboard.Key.D) ||
                   Keyboard.IsKeyPressed(Keyboard.Key.Space))
                {
                    ResetGame();
                }
            }
            else if (isPaused)
            {
                if (Mouse.IsButtonPressed(Mouse.Button.Left))
                {
                    Vector2i mousePosition = Mouse.GetPosition(window);
                    FloatRect startBounds = startSprite.GetGlobalBounds();
                    FloatRect exitBounds = exitSprite.GetGlobalBounds();

                    if (startBounds.Contains(mousePosition.X, mousePosition.Y))
                    {
                        isPaused = false;
                    }
                    else if (exitBounds.Contains(mousePosition.X, mousePosition.Y))
                    {
                        window.Close();
                    }
                }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                if (!wasEscapePressed)
                {
                    isPaused = !isPaused;
                    wasEscapePressed = true;
                }
            }
            else
            {
                wasEscapePressed = false;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.M))
            {
                ToggleSound();
                while (Keyboard.IsKeyPressed(Keyboard.Key.M)) { }
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                currentPlayerSprite = playerSprite1;
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                currentPlayerSprite = playerSprite2;
            }

            window.Clear();
            window.Draw(backgroundSprite);

            if (!isLose && !isPaused)
            {
                DrawPlayer();
                DrawMoney();
            }
            else if (isLose)
            {
                window.Draw(gameOverSprite);
            }
            else if (isPaused)
            {
                DrawMenu();
                DrawPauseIcon();
                DrawLetterMIcon();
                DrawLetterRIcon();
            }

            DrawScore();

            if (!isSoundOn)
            {
                DrawMuteIcon();
            }

            window.Display();
        }
    }
}

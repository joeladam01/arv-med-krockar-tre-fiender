using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OfficeOpenXml;
using System.IO;


namespace Spaceshooter
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<Enemy> enemies;
        static ExcelWorksheet sheet;
        static ExcelPackage package;
        static FileInfo file;
        int raknare;




        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 1500;
            graphics.PreferredBackBufferHeight = 1500;
            graphics.ApplyChanges();
            base.Initialize();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            file = new FileInfo(@"C:\Users\Joel\Desktop\ARvMedKrockMedtrefiender.xlsx");

            package = new ExcelPackage(file);

            sheet = package.Workbook.Worksheets.Add("blad1");

            raknare = 1;

            sheet.Cells[$"A{raknare}"].Value = "Update";
            sheet.Cells[$"B{raknare++}"].Value = "Draw";

        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
           

            // Skapa fiender
            enemies = new List<Enemy>();
            Random random = new Random();
            Texture2D tmpSprite = Content.Load<Texture2D>("images/enemies/mine");
            int posX = 0;
            int posY = 0;
            for (int i = 0; i < 100; i++)
            {

                //int posX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                //int posY = random.Next(0, Window.ClientBounds.Height - tmpSprite.Height);
                if (posX < 1350)
                {
                    posX += 150;
                }
                else
                {
                    posX = 0;
                    posY += 150;
                }

                Mine temp = new Mine(tmpSprite, posX, posY);

                // Lägg till i listan
                enemies.Add(temp);
            }

            tmpSprite = Content.Load<Texture2D>("images/enemies/tripod");
            int posX1 = 50;
            int posY1 = 50;
            for (int i = 0; i < 100; i++)
            {

                //int posX = random.Next(0, Window.ClientBounds.Width - tmpSprite.Width);
                //int posY = random.Next(0, Window.ClientBounds.Height - tmpSprite.Height);
                if (posX1 < 1350)
                {
                    posX1 += 150;
                }
                else
                {
                    posX1 = 50;
                    posY1 += 150;
                }

                Tripod temp = new Tripod(tmpSprite, posX1, posY1);

                // Lägg till i listan
                enemies.Add(temp);
            }

            tmpSprite = Content.Load<Texture2D>("images/enemies/stone");
            int posX2 = 100;
            int posY2 = 100;
            for (int i = 0; i < 100; i++)
            {
                if (posX2 < 1350)
                {
                    posX2 += 150;
                }
                else
                {
                    posX2 = 100;
                    posY2 += 150;
                }
                Stone temp = new Stone(tmpSprite, posX2, posY2);

                // Lägg till i listan
                enemies.Add(temp);
            }

        }

        protected override void UnloadContent()
        {
        }
        
        protected override void Update(GameTime gameTime)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

           

            // Gå igenom alla fiender
            foreach (Enemy e in enemies.ToList())
            {
                
                
                foreach(Enemy e2 in enemies.ToList())
                {
                    if (e.CheckCollision(e2))
                    {
                        e.Changedirection();
                        e2.Changedirection();
                    }
                         
                }

             e.Update(Window);
 
            }
            Console.WriteLine(raknare);

            base.Update(gameTime);
            watch.Stop();
            var elapsedMS = watch.ElapsedTicks;
            if (raknare <= 1200)
            {
                sheet.Cells[$"A{raknare}"].Value = elapsedMS;
            }
            else
            {
                package.Save();
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            foreach (Enemy e in enemies)
                e.Draw(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
            watch.Stop();
            var elapsedMS = watch.ElapsedTicks;
            if (raknare <= 1200)
            {
                sheet.Cells[$"B{raknare++}"].Value = elapsedMS;
            }
        }
    }
}

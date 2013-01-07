using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.Threading;
using System.Diagnostics;

using Wistery.Majong;

namespace Mjai3D
{
    public class Game1 : Game, Component
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;
        SpriteFont spriteFont2;

        Scene3D scene;
        Camera3D camera;

        List<Component> components;
        MajongComponent majong;

        FpsCounter fpsCounter;

        object mutex;

        List<Pai3D> tehaiObjects;
        Texture2D gray;

        List<SelectionType> actionAlternatives;
        List<Rectangle> selectionBoxes;

        public Action Initialized { get; set; }

        public Game1()
        {
            Hoge.Moja("huga");

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            mutex = new object();

            components = new List<Component>();
            majong = new MajongComponent();
            components.Add(majong);

            fpsCounter = new FpsCounter();

            tehaiObjects = new List<Pai3D>();
            actionAlternatives = new List<SelectionType>();
            selectionBoxes = new List<Rectangle>();

            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Pai3D.Initialize(Content);
            Taku.Initialize(Content);
            Pai3DPool.Initialize();
            Algorithm.Initialize();
            Ribou3D.Initialize(Content);
            Ribou3DPool.Initialize();

            SoundEffects.Initialize();

            scene = new Scene3D(GraphicsDevice);

            camera = new Camera3D();

            camera.Position = new Vector3(Taku.SIZE / 2, 200, Taku.SIZE * 1.5f);
            camera.Target = new Vector3(Taku.SIZE / 2, 0, Taku.SIZE / 2);
            camera.UpVector = Vector3.Up;

            camera.FieldOfView = MathHelper.ToRadians(45f);
            camera.AspectRatio = (float)GraphicsDevice.Viewport.Width / GraphicsDevice.Viewport.Height;
            camera.NearPlaneDistance = 1f;
            camera.FarPlaneDistance = 1000f;

            scene.Camera = camera;

            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            spriteFont2 = Content.Load<SpriteFont>("SpriteFont2");
            gray = Content.Load<Texture2D>("gray");

            if (Initialized != null)
                Initialized();
        }

        protected override void Update(GameTime gameTime)
        {
            var K = Keyboard.GetState();
            bool Ctrl = K.IsKeyDown(Keys.LeftControl) || K.IsKeyDown(Keys.RightControl);

            if (Ctrl)
            {
                if (K.IsKeyDown(Keys.Down))
                {
                    camera.Position += Vector3.Down;
                }
                if (K.IsKeyDown(Keys.Up))
                {
                    camera.Position += Vector3.Up;
                }
            }
            else
            {
                if (K.IsKeyDown(Keys.Down))
                {
                    camera.Position += Vector3.Backward;
                }
                if (K.IsKeyDown(Keys.Up))
                {
                    camera.Position += Vector3.Forward;
                }
            }

            if (Ctrl && K.IsKeyDown(Keys.W))
                Exit();

            base.Update(gameTime);          
        }

        protected override void Draw(GameTime gameTime)
        {
            fpsCounter.Draw(gameTime);
            Window.Title = String.Format("{0:F2} fps", fpsCounter.Fps);

            float N = Taku.SIZE;
            float W = Pai3D.WIDTH;
            float H = Pai3D.HEIGHT;
            float D = Pai3D.DEPTH;
            float S = (N - W * 13) / 2;
            float T = (N - W * 6) / 2;

            float[] TEHAI_BASE_X = { S, N - D, S + W * 13, D };
            float[] TEHAI_BASE_Z = { N - D, N - S, D, S };
            float[] TEHAI_DX = { W, 0, -W, 0 };
            float[] TEHAI_DZ = { 0, -W, 0, W };

            float[] KAWA_BASE_X = { T, T + W * 6 + H, T + W * 6, T - H };
            float[] KAWA_BASE_Z = { T + W * 6 + H, T + W * 6, T - H, T };
            float[] KAWA_DX_ROW = { W, 0, -W, 0 };
            float[] KAWA_DZ_ROW = { 0, -W, 0, W };
            float[] KAWA_DX_COL = { 0, H, 0, -H };
            float[] KAWA_DZ_COL = { H, 0, -H, 0 };
            float[] KAWA_REACH_X = { H, 0, -H, 0 };
            float[] KAWA_REACH_Z = { 0, -H, 0, H };

            float[] FURO_BASE_X = { N, N, 0, 0 };
            float[] FURO_BASE_Z = { N, 0, 0, N };
            float[] FURO_DX_TATE = { -W, 0, W, 0 };
            float[] FURO_DZ_TATE = { 0, W, 0, -W };
            float[] FURO_DX_YOKO = { -H, 0, H, 0 };
            float[] FURO_DZ_YOKO = { 0, H, 0, -H };

            float[] FURO_ADJUST_X_YOKO = { H, 0, -H, 0 };
            float[] FURO_ADJUST_Z_YOKO = { 0, -H, 0, H };

            float[] FURO_ADJUST_X_KAKAN = { H, -W, -H, W };
            float[] FURO_ADJUST_Z_KAKAN = { -W, -H, W, H };

            float R = (Taku.SIZE - Ribou3D.WIDTH) / 2;

            float[] RIBOU_X = { R, R + Ribou3D.WIDTH + Ribou3D.DEPTH, R + Ribou3D.WIDTH, R - Ribou3D.DEPTH };
            float[] RIBOU_Y = { R + Ribou3D.WIDTH + Ribou3D.DEPTH, R + Ribou3D.WIDTH, R - Ribou3D.DEPTH, R };

            int SELECTIONBOX_WIDTH = 100;
            int SELECTIONBOX_HEIGHT = 48;
            int SELECTIONBOX_SPACING = 20;

            int SCREEN_WIDTH = GraphicsDevice.Viewport.Width;
            int SCREEN_HEIGHT = GraphicsDevice.Viewport.Height;

            int SCOREBOX_SIZE = SCREEN_WIDTH / 10;
            int SCOREBOX_MARGIN = SCREEN_WIDTH / 30;
            int SCOREBOX_X = SCREEN_WIDTH - SCOREBOX_MARGIN - SCOREBOX_SIZE;
            int SCOREBOX_Y = SCOREBOX_MARGIN;

            Vector2 SCORE_SIZE = spriteFont2.MeasureString("25000");
            int ST = (SCOREBOX_SIZE - (int)SCORE_SIZE.X) / 2;

            int[] SCORE_X = { ST, SCOREBOX_SIZE - (int)SCORE_SIZE.Y, SCOREBOX_SIZE - ST, (int)SCORE_SIZE.Y };
            int[] SCORE_Y = { SCOREBOX_SIZE - (int)SCORE_SIZE.Y, SCOREBOX_SIZE - ST, (int)SCORE_SIZE.Y, ST };

            Dictionary<SelectionType, string> selectionTypeDisplay = new Dictionary<SelectionType, string>(){
                {SelectionType.Chi, "チー"},
                {SelectionType.Pon, "ポン"},
                {SelectionType.Kan, "カン"},
                {SelectionType.Reach, "リーチ"},
                {SelectionType.Ron, "ロン"},
                {SelectionType.Tsumo, "ツモ"},
                {SelectionType.Pass, "パス"}
            };

            lock (mutex)
            {
                GraphicsDevice.Clear(Color.CornflowerBlue);

                /* main drawing procedure */
                scene.Children.Clear();
                tehaiObjects.Clear();
                Pai3DPool.Clear();
                Ribou3DPool.Clear();

                scene.Children.Add(new Taku());

                var M = Mouse.GetState();
                Ray ray = scene.CreateRay(new Point(M.X, M.Y));

                for (int i = 0; i < 4; i++)
                {
                    int iid = (majong.Id + i) % 4;

                    /* 手牌 */
                    var tehai = majong.Tehais[iid].ToList();
                    for (int j = 0; j < tehai.Count; j++)
                    {
                        Pai3D pai3d = Pai3DPool.Get(tehai[j]);
                        pai3d.Position = new Vector3(TEHAI_BASE_X[i] + TEHAI_DX[i] * j, 0, TEHAI_BASE_Z[i] + TEHAI_DZ[i] * j);
                        if (i == 0 && ray.Intersects(pai3d.BoundingBox) != null)
                            pai3d.Position += new Vector3(0, 2, 0);
                        pai3d.Rotation = new Vector3(0, MathHelper.ToRadians(90) * i, 0);
                        scene.Children.Add(pai3d);

                        if (i == 0) tehaiObjects.Add(pai3d);
                    }

                    /* 河 */
                    var kawa = majong.Kawas[iid];
                    for (int j = 0; j < kawa.Count; j++)
                    {
                        Pai3D pai3d = Pai3DPool.Get(kawa[j].pai);
                        Vector3 pos = new Vector3(KAWA_BASE_X[i] + KAWA_DX_COL[i] * (j / 6) + KAWA_DX_ROW[i] * (j % 6), 
                                                  0, 
                                                  KAWA_BASE_Z[i] + KAWA_DZ_COL[i] * (j / 6) + KAWA_DZ_ROW[i] * (j % 6));
                        Vector3 rot = new Vector3(MathHelper.ToRadians(-90), MathHelper.ToRadians(90) * i, 0);
                        if (j == majong.Reaches[iid])
                        {
                            pos += new Vector3(KAWA_REACH_X[i], 0, KAWA_REACH_Z[i]);
                            rot.Y += MathHelper.ToRadians(90);
                        }
                        if (majong.Reaches[iid] != -1 && j > majong.Reaches[iid] && j / 6 == majong.Reaches[iid] / 6)
                            pos += new Vector3(KAWA_REACH_X[i] - KAWA_DX_ROW[i], 0, KAWA_REACH_Z[i] - KAWA_DZ_ROW[i]);
                        pai3d.Position = pos;
                        pai3d.Rotation = rot;
                        scene.Children.Add(pai3d);
                    }

                    /* リーチ棒 */
                    if (majong.Reaches[iid] != -1)
                    {
                        Ribou3D ribou3d = Ribou3DPool.Get();
                        ribou3d.Position = new Vector3(RIBOU_X[i], 0, RIBOU_Y[i]);
                        ribou3d.Rotation = new Vector3(0, MathHelper.ToRadians(90) * i, 0);
                        scene.Children.Add(ribou3d);
                    }

                    /* フーロ */
                    float x = FURO_BASE_X[i];
                    float z = FURO_BASE_Z[i];

                    foreach(Furo f in majong.Furos[iid])
                    {
                        switch (f.type)
                        {
                            case FuroType.Chi:
                            case FuroType.Pon:
                            case FuroType.Daiminkan:
                            case FuroType.Kakan:
                                int paiIndex = (f.target - f.actor + 4) % 4 - 1;
                                for (int k = 0; k < f.consumed.Count + 1; k++)
                                {
                                    if (k == paiIndex)
                                    {
                                        x += FURO_DX_YOKO[i];
                                        z += FURO_DZ_YOKO[i];
                                        Pai3D pai3d = Pai3DPool.Get(f.pai);
                                        pai3d.Position = new Vector3(x + FURO_ADJUST_X_YOKO[i], 0, z + FURO_ADJUST_Z_YOKO[i]);
                                        pai3d.Rotation = new Vector3(MathHelper.ToRadians(-90), MathHelper.ToRadians(90) * (i + 1), 0);
                                        scene.Children.Add(pai3d);

                                        if (f.type == FuroType.Kakan)
                                        {
                                            Pai3D pai3d_ = Pai3DPool.Get(f.pai);
                                            pai3d_.Position = new Vector3(x + FURO_ADJUST_X_KAKAN[i], 0, z + FURO_ADJUST_Z_KAKAN[i]);
                                        }
                                    }
                                    else
                                    {
                                        x += FURO_DX_TATE[i];
                                        z += FURO_DZ_TATE[i];
                                        Pai3D pai3d = Pai3DPool.Get(f.consumed[k - ((k > paiIndex) ? 1 : 0)]);
                                        pai3d.Position = new Vector3(x, 0, z);
                                        pai3d.Rotation = new Vector3(MathHelper.ToRadians(-90), MathHelper.ToRadians(90) * i, 0);
                                        scene.Children.Add(pai3d);
                                    }
                                }
                                break;
                            case FuroType.Ankan:
                                throw new NotImplementedException("Game1#Draw: draw Ankan is not yet implemented.(not so hard, but I'm lazy)");
                        }
                    }
                }

                scene.Draw();

                /* 2D Shapes */

                spriteBatch.Begin();
                selectionBoxes.Clear();

                spriteBatch.Draw(gray, 
                                 new Rectangle(SCOREBOX_X, 
                                               SCOREBOX_Y, 
                                               SCOREBOX_SIZE,
                                               SCOREBOX_SIZE), 
                                 Color.Gray * 0.5f);

                string baStr = String.Format("{0}-{1}-{2}", majong.Kyoku.ToString(), majong.Honba.ToString(), majong.Kyotaku.ToString());
                Vector2 baStrSize = spriteFont2.MeasureString(baStr);
                spriteBatch.DrawString(spriteFont2,
                                       baStr,
                                       new Vector2(SCOREBOX_X + (SCOREBOX_SIZE - baStrSize.X) / 2, SCOREBOX_Y + (SCOREBOX_SIZE - baStrSize.Y) / 2),
                                       Color.White);

                /* Scores */
                for(int i = 0; i < 4; i++) {
                    int iid = (i + majong.Id) % 4;
                    string sc = majong.Scores[iid].ToString();
                    Vector2 strSize = spriteFont2.MeasureString(sc);
                    spriteBatch.DrawString(spriteFont2,
                                           sc,
                                           new Vector2(SCOREBOX_X + SCORE_X[i],
                                                       SCOREBOX_Y + SCORE_Y[i]),
                                           Color.White, 
                                           MathHelper.ToRadians(-90) * i,
                                           Vector2.Zero,
                                           1,
                                           SpriteEffects.None,
                                           0);
                }

                /* Action Alternatives */
                for(int i = 0; i < actionAlternatives.Count; i++)
                {
                    var alt = actionAlternatives[i];
                    int x = SCREEN_WIDTH * 4/5 - i * (SELECTIONBOX_WIDTH + SELECTIONBOX_SPACING);
                    int y = SCREEN_HEIGHT * 4/5;
                    Rectangle rect = new Rectangle(x, y, SELECTIONBOX_WIDTH, SELECTIONBOX_HEIGHT);
                    selectionBoxes.Add(rect);
                    spriteBatch.Draw(gray, rect, Color.Gray * 0.75f);
                    string str = selectionTypeDisplay[alt];
                    Vector2 strSize = spriteFont.MeasureString(str);
                    spriteBatch.DrawString(spriteFont, str, new Vector2(x + (SELECTIONBOX_WIDTH - strSize.X) / 2, y + (SELECTIONBOX_HEIGHT - strSize.Y) / 2), Color.White);
                }
                spriteBatch.End();

                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }

            base.Draw(gameTime);
        }

        Point GetClick()
        {
            while (true)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                    break;
                Thread.Sleep(1);
            }
            while (true)
            {
                if (Mouse.GetState().LeftButton == ButtonState.Released)
                    break;
                Thread.Sleep(1);
            }
            var M = Mouse.GetState();
            return new Point(M.X, M.Y);
        }

        Selection1 Select1Loop(List<SelectionType> alternatives)
        {
            while (true)
            {
                Console.WriteLine("Game1#Select1Loop");

                Point click = GetClick();
                Ray ray = scene.CreateRay(click);

                lock (mutex)
                {
                    Console.WriteLine(selectionBoxes.Count);
                    for (int i = 0; i < selectionBoxes.Count; i++)
                    {
                        var box = selectionBoxes[i];

                        if (click.X > box.Left && click.X < box.Right && click.Y > box.Top && click.Y < box.Bottom)
                        {
                            switch (alternatives[i])
                            {
                                case SelectionType.Kan:
                                    throw new NotImplementedException("Game1#Select1: Ankan is not yet implemented");
                                case SelectionType.Reach:
                                    return new Reach();
                                case SelectionType.Tsumo:
                                    return new Tsumo();
                            }
                        }
                    }

                    int ix = tehaiObjects.FindIndex(pai3d => ray.Intersects(pai3d.BoundingBox) != null);

                    Console.WriteLine(ix);

                    if (ix != -1)
                    {
                        return new Dahai(tehaiObjects[ix].pai, ix == tehaiObjects.Count - 1);
                    }
                }
            }
        }

        public Selection1 Select1(List<SelectionType> alternatives)
        {
            Console.WriteLine("Game#Select1");

            actionAlternatives = alternatives.ToList();

            Selection1 ans = Select1Loop(alternatives);
            
            actionAlternatives.Clear();

            Debug.Assert(ans != null);

            return ans;
        }

        Tuple<Pai, Pai> Select2PonChiLoop()
        {
            
            List<Pai> indices = new List<Pai>();
            while (true)
            {
                Point click = GetClick();
                Ray ray = scene.CreateRay(click);

                lock (mutex)
                {
                    int ix = tehaiObjects.FindIndex(pai3d => ray.Intersects(pai3d.BoundingBox) != null);
                    Console.WriteLine(ix);
                    if (ix != -1)
                    {
                        indices.Add(tehaiObjects[ix].pai);
                        if (indices.Count == 2)
                            break;
                    }
                }
            }
            return Tuple.Create(indices[0], indices[1]);
        }

        Selection2 Select2Loop(List<SelectionType> alternatives) {
            while (true)
            {
                Console.WriteLine("Game1#Select2Loop");

                Point click = GetClick();

                List<Rectangle> selectionBoxes_;
                lock (mutex)
                {
                    selectionBoxes_ = selectionBoxes.ToList();
                }
                for (int i = 0; i < selectionBoxes_.Count; i++)
                {
                    Rectangle box = selectionBoxes_[i];
                    if (click.X > box.Left && click.X < box.Right && click.Y > box.Top && click.Y < box.Bottom)
                    {
                        switch (alternatives[i])
                        {
                            case SelectionType.Chi:
                                {
                                    actionAlternatives.Clear();
                                    var tmp = Select2PonChiLoop();
                                    return new Chi(tmp.Item1, tmp.Item2);
                                }
                            case SelectionType.Pon:
                                {
                                    actionAlternatives.Clear();
                                    var tmp = Select2PonChiLoop();
                                    return new Pon(tmp.Item1, tmp.Item2);
                                }
                            case SelectionType.Kan:
                                return new Daiminkan();
                            case SelectionType.Ron:
                                return new Ron();
                            case SelectionType.Pass:
                                return new Pass();
                        }
                    }
                }
                
            }
        }

        public Selection2 Select2(List<SelectionType> alternatives)
        {
            Console.WriteLine("Gamae#Select2");

            actionAlternatives = alternatives.ToList();

            Selection2 ans = Select2Loop(alternatives);

            actionAlternatives.Clear();

            Debug.Assert(ans != null);
            return ans;
        }

        public void onStartGame(int id, List<string> names)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onStartGame(id, names);
                }
            }
        }

        public void onStartKyoku(Pai bakaze, int kyoku, int honba, int kyotaku, int oya, Pai doraMarker, List<List<Pai>> tehais)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onStartKyoku(bakaze, kyoku, honba, kyotaku, oya, doraMarker, tehais);
                }
            }
        }

        public void onTsumo(int actor, Pai pai)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onTsumo(actor, pai);
                }
            }
        }

        float pan(int iid)
        {
            if (iid == 1) return 1f;
            if (iid == 3) return -1f;
            return 0f;
        }

        public void onDahai(int actor, Pai pai, bool tsumogiri)
        {
            SoundEffects.Dahai(pan(actor - majong.Id + 4) % 4);
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onDahai(actor, pai, tsumogiri);
                }
            }
        }

        public void onReach(int actor)
        {
            SoundEffects.Reach(pan(actor - majong.Id + 4) % 4);
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onReach(actor);
                }
            }
        }

        public void onReachAccepted(int actor, List<int> deltas, List<int> scores)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onReachAccepted(actor, deltas, scores);
                }
            }
        }

        public void onPon(int actor, int target, Pai pai, List<Pai> consumed)
        {
            SoundEffects.Pon(pan(actor - majong.Id + 4) % 4);
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onPon(actor, target, pai, consumed);
                }
            }
        }

        public void onChi(int actor, int target, Pai pai, List<Pai> consumed)
        {
            SoundEffects.Chi(pan(actor - majong.Id + 4) % 4);
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onChi(actor, target, pai, consumed);
                }
            }
        }

        public void onKan(int actor, int target, Pai pai, List<Pai> consumed)
        {
            SoundEffects.Kan(pan(actor - majong.Id + 4) % 4);
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onKan(actor, target, pai, consumed);
                }
            }
        }

        public void onHora(int actor, int target, Pai pai, List<Pai> horaTehais, List<YakuN> yakus, int fu, int fan, int horaPoints, List<int> deltas, List<int> scores)
        {
            if (actor == target)
            {
                SoundEffects.Tsumo(pan(actor - majong.Id + 4) % 4);
            }
            else
            {
                SoundEffects.Ron(pan(actor - majong.Id + 4) % 4);
            }
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onHora(actor, target, pai, horaTehais, yakus, fu, fan, horaPoints, deltas, scores);
                }
            }
        }

        public void onRyukyoku(string reason, List<List<Pai>> tehais, List<bool> tenpais, List<int> deltas, List<int> scores)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onRyukyoku(reason, tehais, tenpais, deltas, scores);
                }
            }
        }

        public void onEndKyoku()
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onEndKyoku();
                }
            }
        }

        public void onError(string message)
        {
            lock (mutex)
            {
                foreach (var component in components)
                {
                    component.onError(message);
                }
            }
        }
    }
}

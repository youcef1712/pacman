using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PacMan.Content;
using static PacMan.SauvegardePartie;
using static PacMan.TransformeXML;

namespace PacMan;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    public const int windowWidth = 850;
    public const int windowHeight = 800;
    
    
    private FormulaireJoueur formulaireJoueur;
    private DonneesJoueur joueur;
    private SpriteFont font;
    
    private Menu menu;
    private EtatJeu etatJeuCourant;

    private int score;
    private float timer; 
  
    
    
    private float distanceParcourue =0f;
    float optimalDistance = 0f; // Distance optimale initialisée à 0 au debut pour etre recuperer avec XmlReader apres
    float optimalTime = 0f; // La meme chose
    
    private int nbpartie;
    
    private DateTime? derniereSauvegarde = null;
    private bool partieSauvegardee = false;
    
    //pour generer le resultat de chaque joueure html
    private string xmlPath = "../../../XmlGenerer/sauvegarde.xml";
    private string xsltPath = "../../../xslt/ResultatHtml.xslt";
    private string outputPath = "../../../HtmlGenerer/resultat.html";
    
    //Pour generer la fiche xml des partie
    private string xsltPath2 = "../../../xslt/ResultatXml.xslt";
    private string outputPath2 = "../../../XmlGenerer/Partie.xml";
    
    //classement de score en fichier html
    private string xsltPath3 = "../../../xslt/highScore.xslt";
    private string outputPath3 = "../../../HtmlGenerer/HighScore.html";
    
    
   //Les données sérialisation pour le fichier etatJeu.xml
    [XmlRoot("JeuEtat", Namespace = "http://www.monjeu.com/jeuPacMan")]
    public class JeuEtat
    {
        [XmlElement("etatJeu", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public EtatJeu Etat { get; set; }

        [XmlElement("score", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public int Score { get; set; }

        [XmlElement("vies", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public int Vies { get; set; }

        [XmlElement("tempsJoue", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public float TempsJoue { get; set; }

        [XmlElement("nomJoueur", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public string NomJoueur { get; set; }

        [XmlElement("distanceParcourue", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public float DistanceParcourue { get; set; }

        [XmlElement("optimalDistance", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public float OptimalDistance { get; set; }

        [XmlElement("optimalTime", Namespace = "http://www.monjeu.com/jeuPacMan")]
        public float OptimalTime { get; set; }
        
    }

    
    //les etats du jeu
    public enum EtatJeu
    {
        Formulaire,
        Menu,
        Jeu,
        Perdu,
        Gagne,
        Quitter
    }
    //pour sérialiser ces données dans dans le fichier etat_jeu.xml
    
    public void SauvegarderEtatJeu(string fichier)
    {
        var jeuEtat = new JeuEtat
        {
            Etat = etatJeuCourant,
            Score = score,
            Vies = ListeCoeurs.Count,
            TempsJoue = (float)Math.Round(timer, 2), //arrondir la virgule
            NomJoueur = joueur.Nom,
            DistanceParcourue = (float)Math.Round(distanceParcourue, 2),
            OptimalDistance = optimalDistance,
            OptimalTime = optimalTime,
            
        };

        var xmlNamespaces = new XmlSerializerNamespaces();
        xmlNamespaces.Add("pc", "http://www.monjeu.com/jeuPacMan");

        var serializer = new XmlSerializer(typeof(JeuEtat));
        using (var writer = new StreamWriter(fichier))
        {
            serializer.Serialize(writer, jeuEtat, xmlNamespaces);
        }
    }


    public void MettreAJouroptimalValue(string fichier)//XmlReader pour recuperer les valeurs optimal
    {
        using (XmlReader reader = XmlReader.Create(fichier))
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(reader);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("pc", "http://www.monjeu.com/jeuPacMan");

            XmlNode optimalDistanceNode = doc.SelectSingleNode("//pc:optimalDistance", nsmgr);
            if (optimalDistanceNode != null)
            {
                optimalDistance = float.Parse(optimalDistanceNode.InnerText);
            }

            XmlNode optimalTimeNode = doc.SelectSingleNode("//pc:optimalTime", nsmgr);
            if (optimalTimeNode != null)
            {
                optimalTime = float.Parse(optimalTimeNode.InnerText);
            }
        }
        Console.WriteLine($"optimalDistance: {optimalDistance}");
        Console.WriteLine($"optimalTime: {optimalTime}");
    }
   




    private Carte carte;
    private Texture2D TextureMur;
    
    private Pacman Pacman;
    private Texture2D TextureOuverte;
    private Texture2D TextureFermee;
    
    private List<Ennemi> Ennemis;
    private Texture2D TextureEnnemi;

    private Texture2D TexturePointAmanger;
    private listePoints listePoints;
    private List<PointAmanger> ListePoints;

    private Texture2D TextureCoeur;
    private List<Texture2D> ListeCoeurs;

    private Texture2D textureFormulaire;
    
    
    private SoundEffect sonDeplacement;
    private SoundEffect sonEnnemi;
    private SoundEffectInstance instanceSonEnnemi; 
    
    private double tempsDepuisDerniereCollision = 0;
    private const double cooldownCollision = 2.0; //temps ou le pacman reste invincible apres la colision d un fantome
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        
        _graphics.PreferredBackBufferWidth = windowWidth;
        _graphics.PreferredBackBufferHeight = windowHeight;
    }

    protected override void Initialize()
    {   score = 0;
        timer = 0;
        etatJeuCourant = EtatJeu.Formulaire;
        menu = new Menu();
        base.Initialize();
        MettreAJouroptimalValue("../../../xml/etat_Jeu.xml");
    }
    
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        font = Content.Load<SpriteFont>("File");
        formulaireJoueur = new FormulaireJoueur(font);
        
        menu.LoadContent(Content);
        
        
        var jeu = XmlLoader.LoadFromXml<Jeu>("../../../xml/PacMan.xml");
        
        carte = jeu.Carte;
        TextureMur = Content.Load<Texture2D>("mur");
        carte.Initialiser(TextureMur);
        carte.ConvertirLignesEnGrille();
        
        TexturePointAmanger = Content.Load<Texture2D>("Amanger");
        listePoints = new listePoints(carte.GetGrille(), TexturePointAmanger);
        ListePoints = listePoints.GetListePoint();
        
        Pacman = jeu.Pacman;
        TextureOuverte = Content.Load<Texture2D>("pacman");
        TextureFermee = Content.Load<Texture2D>("pacmanFerme");
        sonDeplacement = Content.Load<SoundEffect>("sonDeplacement");
        Pacman.Initialiser(TextureOuverte, TextureFermee, sonDeplacement,listePoints.GetListePoint());

        Ennemis = jeu.Ennemis;
        TextureEnnemi = Content.Load<Texture2D>("fantomeRouge");
        sonEnnemi = Content.Load<SoundEffect>("sonEnnemi");
        instanceSonEnnemi = sonEnnemi.CreateInstance();
        foreach (var e in Ennemis)
        {
            e.Initialiser(TextureEnnemi);
        }
        
        TextureCoeur = Content.Load<Texture2D>("coeur");
        ListeCoeurs = new List<Texture2D> { TextureCoeur, TextureCoeur, TextureCoeur };
        
        textureFormulaire = Content.Load<Texture2D>("back_formulaire");
        
    }
    private void GererCollisions(GameTime gameTime)
    {
        // pour calculer le temps apres la derniere colision
        tempsDepuisDerniereCollision += gameTime.ElapsedGameTime.TotalSeconds;
        
        for (int i = ListePoints.Count - 1; i >= 0; i--)
        {
            if (Pacman.GererCollisionAvec(ListePoints[i]))
            {
                ListePoints.RemoveAt(i); // Supprimez le point mangé
                score += 10;
            }
        }

        if (tempsDepuisDerniereCollision >= cooldownCollision)
        {
            // Collision entre Pac-Man et les fantômes
            foreach (var e in Ennemis)
            {
                if (Pacman.GererCollisionAvec(e))
                {
                    ListeCoeurs.RemoveAt(ListeCoeurs.Count - 1);
                    sonEnnemi.Play();
                    tempsDepuisDerniereCollision = 0;
                }
            }
        }
    }
    

    
    protected override void Update(GameTime gameTime)
    {
        
      

        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        switch (etatJeuCourant)
        {
            case EtatJeu.Formulaire:
       
               
                joueur = formulaireJoueur.RecupererDonneesJoueur(Keyboard.GetState());
                if (joueur != null && !string.IsNullOrEmpty(joueur.Nom) && !string.IsNullOrEmpty(joueur.Prenom) && joueur.Age > 0)
                {
                    // pour sauvegarder les données du joueur dans le fichier Sauvegarde.xml
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add("", "http://www.monjeu.com/jeuPacMan"); 
                    XmlLoader.Save("../../../XmlGenerer/DonneesJoueur.xml",joueur,ns);
                    etatJeuCourant = EtatJeu.Menu;
                }
                break;
            case EtatJeu.Jeu:
                
                Pacman.Update(gameTime, carte.GetGrille());
                timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                foreach (var e in Ennemis)
                {
                    e.Update(gameTime, carte.GetGrille());
                }

                distanceParcourue=Pacman.totalDistance;//pour recuperer la distance parcourue
                
                // Gestion des collisions
                GererCollisions(gameTime);

                //Cas de la victoire
                if (ListePoints.Count == 0)
                {
                    menu.SetGameOver(true);//le menu game over ou gagné sont le meme
                    nbpartie++;
                    etatJeuCourant = EtatJeu.Gagne;
                    SauvegarderEtatJeu("../../../xml/etat_jeu.xml");

                } else if (ListeCoeurs.Count == 0) //cas gameover
                {
                    menu.SetGameOver(true);
                    nbpartie++;
                    instanceSonEnnemi.Stop();
                    etatJeuCourant = EtatJeu.Perdu;
                    SauvegarderEtatJeu("../../../xml/etat_jeu.xml");



                }

                break;
            case EtatJeu.Perdu:
                menu.Update(); 
                instanceSonEnnemi.Stop(); 
                Pacman.instanceSon?.Stop(); 
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (menu.SelectedOption == 0) // Rejouer
                    {
                        Initialize(); 
                        etatJeuCourant = EtatJeu.Jeu; // Retourne a etat jeu
                        menu.SetGameOver(false);
                    }
                    else if (menu.SelectedOption == 1) // Quitter
                    {
                        etatJeuCourant = EtatJeu.Quitter;
                    }
                }
                break;
            case EtatJeu.Gagne:
                MettreAJouroptimalValue("../../../xml/etat_jeu.xml");
                Console.WriteLine($"optimalDistance: {optimalDistance}");
                Console.WriteLine($"optimalTime: {optimalTime}");
                
                menu.Update(); 
                instanceSonEnnemi.Stop(); 
                Pacman.instanceSon?.Stop(); 
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    if (menu.SelectedOption == 0) 
                    {
                        Initialize(); 
                        etatJeuCourant = EtatJeu.Jeu; 
                        menu.SetGameOver(false);
                    }
                    else if (menu.SelectedOption == 1) 
                    {
                        etatJeuCourant = EtatJeu.Quitter;
                    }
                }
                break; 

            case EtatJeu.Menu: 
                menu.Update();
                if (Keyboard.GetState().IsKeyDown(Keys.Enter)) 
                {
                    if (menu.SelectedOption == 0)
                        etatJeuCourant = EtatJeu.Jeu; 
                    else if (menu.SelectedOption == 1)
                        etatJeuCourant = EtatJeu.Quitter; 
                }
                break;
            case EtatJeu.Quitter:
                Exit();
                break;
        }
        if ((etatJeuCourant == EtatJeu.Perdu || etatJeuCourant == EtatJeu.Gagne))
        {
            if (!partieSauvegardee) // Si la sauvegarde n est pas faites 
            {
                AjouterPartie(joueur.Nom, nbpartie, DateTime.Now, etatJeuCourant.ToString(), score);
                TransformerXMLVersHTML(xmlPath, xsltPath, outputPath);
                TransformerXMLVersXML(xmlPath, xsltPath2, outputPath2);
                TransformerXMLVersHTML(xmlPath, xsltPath3, outputPath3);
                TransformerXMLVersHTML("../../../xml/etat_jeu.xml","../../../xslt/etat_jeu.xsl","../../../HtmlGenerer/etat_jeu.html");
                partieSauvegardee = true; // Marque cette partie comme sauvegardée
            }
        }
        else
        {
            
            partieSauvegardee = false;
        }



        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        switch (etatJeuCourant)
        {
            case EtatJeu.Formulaire:
                _spriteBatch.Draw(textureFormulaire, new Rectangle(0, 0, windowWidth, windowHeight), Color.White);
               
                formulaireJoueur.Draw(_spriteBatch);
                break;
            case EtatJeu.Menu:
                menu.Draw(_spriteBatch, new Vector2(350, 400), Color.White, Color.Yellow); // Dessiner le menu
                break;

            case EtatJeu.Jeu:
                // Dessiner l'état de jeu 
                carte.Draw(_spriteBatch);
                listePoints.Draw(_spriteBatch, 15);
                Pacman.Draw(_spriteBatch);
                
                string timeText = $"Time: {timer:F2}"; 
                _spriteBatch.DrawString(font, timeText, new Vector2(300, 10), Color.Black);
                
                _spriteBatch.DrawString(font, $"Score: {score}", new Vector2(10, 10), Color.Black); 

                foreach (var e in Ennemis)
                {
                    e.Draw(_spriteBatch, 30);
                }
                // pour afficher les coeurs
                for (int i = 0; i < ListeCoeurs.Count; i++)
                {
                    
                    Vector2 position = new Vector2(windowWidth - (TextureCoeur.Width + 10) * (i + 1), 10);
                    _spriteBatch.Draw(ListeCoeurs[i], position, Color.White);
                }
                break;

            case EtatJeu.Perdu:
                menu.Draw(_spriteBatch, new Vector2(350, 400), Color.White, Color.Yellow); // Dessiner les options
                break;
            case EtatJeu.Gagne:
                menu.Draw(_spriteBatch, new Vector2(350, 400), Color.White, Color.Yellow); // Dessiner les options
                break;
        }

        _spriteBatch.End();



        base.Draw(gameTime);
    }
    
}
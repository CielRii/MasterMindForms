///ETML
///Auteure: Sarah Dongmo
///Date de création: 23.11.23
///Dernière modification: 11.01.24

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Runtime.InteropServices;
using System.Collections;
using System.Runtime.CompilerServices;

namespace MasterMind__Forms_
{

    

    public partial class Main : Form
    {
        /*VARIABLES COMMUNES A DIFFERENTES ZONES*/
        const int BUTTON_SIZE = 40,  PANEL_SIZE = 40; //panelSize, buttonSize, controlSize, solutionLenght
        const int CONTROL_SIZE = 12;
        int x = 1;
        int y = 1;

        /*AUTRES VARIABLES*/
        const int SOLUTION_LENGHT = 4;
        int choiceLenght = 7;
        int beforeRightLenght;
        int indexSolution = 1;
        int indexUser = 1;
        int indexControlPnl = 1;
        int indexControlBtn = 1;
        int indexUserX = 0;
        int indexUserY = 0;
        int control = 0;
        int xCheck = 0;
        int yCheck = 1;
        int roundControl = 0;

        Random random = new Random();
        Color[] randomSolution;
        Color[] randomChoice;

        Button selectedColorButton = new Button();
        Button solutionBtn;
        Button btn;
        Panel panel;
        Button[] allChoiceBtn;
        Button[,] allUserBtn;
        Button userBtn;
        Button controlBtn;

        bool colorblind = false;
        bool frenchOption = true;      
        bool endGame = false;

        private EndMessage EndMessage;
        ColorPositionCheck colorPositionCheck = new ColorPositionCheck();

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            /*INTRODUCTION*/
            DialogResult introductionMessage = MessageBox.Show
            ("Bienvenue! Vous vous apprêtez à jouer à MASTERMIND.\n" +
            "Choisisez votre langue ! \"Oui\" pour le français, \"Non\" pour l'anglais.\n\n"
            + "Welcome ! You are about to start the game MASTERMIND.\n" +
            "Please choose your langage!\"Yes\" for French, \"Non\" for English.","Introduction", MessageBoxButtons.YesNo);
            switch (introductionMessage)
            {
                case DialogResult.Yes:
                    frenchOption = true;
                    break;
                case DialogResult.No:
                    frenchOption = false;
                    break;
            }

            /*QUESTION 1 - DALTONIEN*/
            if (frenchOption)
            {
                DialogResult colorblindMessage = MessageBox.Show
                ("Êtes-vous daltonien ?", "Question 1", MessageBoxButtons.YesNo);
                switch (colorblindMessage)
                {
                    case DialogResult.Yes:
                        colorblind = true;
                        break;
                    case DialogResult.No:
                        colorblind = false;
                        break;
                }
            }
            else
            {
                DialogResult colorblindMessage = MessageBox.Show
                ("Are you colorblind ?", "Question 1", MessageBoxButtons.YesNo);
                switch (colorblindMessage)
                {
                    case DialogResult.Yes:
                        colorblind = true;
                        break;
                    case DialogResult.No:
                        colorblind = false;
                        break;
                }
            }

            /*QUESTION 2 - LONGUEUR COULEURS*/
            string numberOfColorsMessage;
            if (frenchOption)
            {
                do
                {
                    //AUTRE VERSION D'ÉCRITURE: var userLenght = Interaction.InputBox("Avec combien de couleurs voulez-vous jouer ?", "", Convert.ToString(choiceLenght), -1, -1);
                    numberOfColorsMessage = Microsoft.VisualBasic.Interaction.InputBox("Avec combien de couleurs voulez-vous jouer ?\n"
                    + "Choississez un nombre entre 4 et 9 compris !", "Question 2", Convert.ToString(choiceLenght));
                    choiceLenght = Convert.ToInt32(numberOfColorsMessage);

                } while (Convert.ToInt32(numberOfColorsMessage) < 4 || Convert.ToInt32(numberOfColorsMessage) > 9);
            }
            else
            {
                do
                {
                    //AUTRE VERSION D'ÉCRITURE: var userLenght = Interaction.InputBox("Avec combien de couleurs voulez-vous jouer ?", "", Convert.ToString(choiceLenght), -1, -1);
                    numberOfColorsMessage = Microsoft.VisualBasic.Interaction.InputBox("How many color do you want to play with ?\n"
                    + "Choose a number between 4 and 9 !", "Question 2", Convert.ToString(choiceLenght));
                    choiceLenght = Convert.ToInt32(numberOfColorsMessage);

                } while (Convert.ToInt32(numberOfColorsMessage) < 4 || Convert.ToInt32(numberOfColorsMessage) > 9);
            }



            /*RANDOMNISATION DE LA SOLUTION*/
            //SOLUTION AVEC REDONDANCE: Color[] randomColors = new Color[] { Color.Green, Color.Yellow, Color.White, Color.Red, Color.Blue, Color.Magenta, Color.Cyan };
            //Color randomColor = randomColors[random.Next(randomColors.Length)];
            List<Color> unshuffledRandomColors;
            List<Color> shuffledRandomColors;
            if (colorblind == true)
            {
                unshuffledRandomColors = new List<Color>() { Color.SkyBlue, Color.CadetBlue, Color.White, Color.DarkSeaGreen,
                    Color.Yellow, Color.PowderBlue, Color.Turquoise, Color.DarkGreen, Color.YellowGreen };
                shuffledRandomColors = new List<Color>();
            }
            else
            {
                unshuffledRandomColors = new List<Color>() { Color.Green, Color.Yellow, Color.White, Color.Red,
                    Color.Blue, Color.Magenta, Color.Cyan, Color.DarkOrange, Color.DeepPink };
                shuffledRandomColors = new List<Color>();
            }

            for (int p = unshuffledRandomColors.Count; p > 0; p--) //Boucle de randomnisation sans redondance
            {
                int k = random.Next(p);
                Color temp = unshuffledRandomColors[k];
                shuffledRandomColors.Add(temp);
                unshuffledRandomColors.RemoveAt(k);
            }
            randomSolution = new Color[SOLUTION_LENGHT];

            for (int m = 0; m < SOLUTION_LENGHT; m++)
            {
                solutionBtn = new Button(); //Sinon on se retrouve avec un bouton
                randomSolution[m] = shuffledRandomColors[m];
                solutionBtn.BackColor = randomSolution[m];

                solutionBtn.FlatStyle = FlatStyle.Popup;
                solutionBtn.Name = "solution" + indexSolution;
                solutionPanel.Controls.Add(solutionBtn);

                solutionBtn.Height = solutionBtn.Width = BUTTON_SIZE;
                solutionBtn.Location = new System.Drawing.Point(x, y);

                x += BUTTON_SIZE + 10;
                indexSolution++;
            }
            x = 1;
            y = 1;


            /*GESTION ZONE DE CONTRÔLE*/
            for (int i = 0; i < 10; i++)
            {
                panel = new Panel();
                panel.Height = panel.Width = PANEL_SIZE;
                panel.Location = new System.Drawing.Point(x, y);

                panel.BackColor = Color.LightGoldenrodYellow;
                panel.Name = "panel" + indexControlPnl;
                controlPanel.Controls.Add(panel);
                

                y += PANEL_SIZE + 10;

                int ctrlX = 3;
                int ctrlY = 3;
                for (int j = 0; j < 2; j++)
                {

                    for (int k = 0; k < 2; k++)
                    {
                        controlBtn = new Button();
                        controlBtn.Height = controlBtn.Width = CONTROL_SIZE;
                        controlBtn.Location = new System.Drawing.Point(ctrlX, ctrlY);

                        controlBtn.BackColor = Color.LightGray;
                        controlBtn.FlatStyle = FlatStyle.Popup;
                        controlBtn.Name = "controlBtn" + indexControlBtn + "Panel" + indexControlPnl;
                        panel.Controls.Add(controlBtn);

                        ctrlX += CONTROL_SIZE + 10;
                        if (ctrlX >= (CONTROL_SIZE + 10) * 2)
                        {
                            ctrlX = 3;
                            ctrlY += CONTROL_SIZE + 10;
                        }
                        indexControlBtn++;

                        //Problème - affichage d'un unique bouton:
                        //allControlBtn[k, j] = userBtn;
                        //controlPanel.Controls.Add(allControlBtn[k, j]);
                    }
                }
                indexControlPnl++;
                indexControlBtn = 1;
            }
            x = 1;
            y = 1;

            /*GESTION ZONE DE CHOIX*/
            /*Correspondance couleur de fond et 1ère partie du nom du bouton*/

            //Ajout des couleurs en fonction de ceux de la solution
            List<Color> keyList= new List<Color>(choiceLenght);
            keyList.Add(randomSolution[0]);
            keyList.Add(randomSolution[1]);
            keyList.Add(randomSolution[2]);
            keyList.Add(randomSolution[3]);

            beforeRightLenght = choiceLenght - SOLUTION_LENGHT;
            if (colorblind == true)
            {
                if (!keyList.Contains(Color.SkyBlue) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.SkyBlue); 
                    beforeRightLenght--;
                }              
                if (!keyList.Contains(Color.CadetBlue) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.CadetBlue);
                    beforeRightLenght--;
                }                  
                if (!keyList.Contains(Color.White) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.White);
                    beforeRightLenght--;
                }                      
                if (!keyList.Contains(Color.DarkSeaGreen) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.DarkSeaGreen);
                    beforeRightLenght--;
                }               
                if (!keyList.Contains(Color.Yellow) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Yellow); 
                    beforeRightLenght--;
                }                   
                if (!keyList.Contains(Color.PowderBlue) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.PowderBlue);
                    beforeRightLenght--;
                }
                if (!keyList.Contains(Color.Turquoise) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Turquoise); 
                    beforeRightLenght--;
                }
                if (!keyList.Contains(Color.YellowGreen) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.YellowGreen);
                    beforeRightLenght--;
                }             
                if (!keyList.Contains(Color.DarkGreen) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.DarkGreen); 
                    beforeRightLenght--;
                }                  
            }
            else
            {
                if (!keyList.Contains(Color.Green) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Green);
                    beforeRightLenght--;
                }
                if (!keyList.Contains(Color.Yellow) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Yellow);
                    beforeRightLenght--;
                }                       
                if (!keyList.Contains(Color.White) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.White);
                    beforeRightLenght--;
                }          
                if (!keyList.Contains(Color.Red) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Red);
                    beforeRightLenght--;
                }   
                if (!keyList.Contains(Color.Blue) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Blue);
                    beforeRightLenght--;
                }          
                if (!keyList.Contains(Color.Magenta) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Magenta);
                    beforeRightLenght--;
                }          
                if (!keyList.Contains(Color.Cyan) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.Cyan);
                    beforeRightLenght--;
                }           
                if (!keyList.Contains(Color.DarkOrange) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.DarkOrange);
                    beforeRightLenght--;
                }
                if (!keyList.Contains(Color.DeepPink) && beforeRightLenght > 0)
                {
                    keyList.Add(Color.DeepPink);
                    beforeRightLenght--;
                }
            }            
            randomChoice = new Color[choiceLenght];

            //Randomnisation des éléments zone de choix
            Random rng = new Random();
            int n = keyList.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Color value = keyList[k];
                keyList[k] = keyList[n];
                keyList[n] = value;
            }
            for (int i = 0; i < keyList.Count; i++)
            {
                randomChoice[i] = keyList[i];
            }

            /*AUTRE SOLUTION:
             * while (list.Count != 0)
            {
                var index = rnd.Next(0, list.Count);
                randomizedList.Add(list[index]);
                list.RemoveAt(index);
            }*/

            for (int i = 0; i < choiceLenght; i++)
            {

                btn = new Button(); //Déclaration dans la boucle sinon on se retrouve avec un bouton
                this.btn.Click += new System.EventHandler(this.UserBtn_Click);
                btn.Height = btn.Width = BUTTON_SIZE;
                btn.Location = new System.Drawing.Point(x, y);
                btn.BackColor = randomChoice[i];
                btn.Name = Convert.ToString(randomChoice[i]).ToLower() + "ChoiceBtn";
                btn.FlatStyle = FlatStyle.Popup;
                allChoiceBtn = new Button[choiceLenght];
                allChoiceBtn[i] = btn;

                choicePanel.Controls.Add(allChoiceBtn[i]);

                x += BUTTON_SIZE + 15;
                switch (choiceLenght)
                {
                    case 5:
                        if (x >= (BUTTON_SIZE + 15) * 3)
                        {
                            x = 35;
                            y += BUTTON_SIZE + 30;
                        }
                        break;
                    case 6:
                        if (x >= (BUTTON_SIZE + 15) * 3)
                        {
                            x = 0;
                            y += BUTTON_SIZE + 30;
                        }
                        break;
                    case 8:
                        if (x >= (BUTTON_SIZE + 15) * 4)
                        {
                            x = 0;
                            y += BUTTON_SIZE + 30;
                        }
                        break;
                    case 9:
                        if (x >= (BUTTON_SIZE + 15) * 5)
                        {
                            x = 28;
                            y += BUTTON_SIZE + 30;
                        }
                        break;
                    default:
                        if (x >= (BUTTON_SIZE + 15) * 4)
                        {
                            x = 35;
                            y += BUTTON_SIZE + 30;
                        }
                        break;
                }
            }
            x = 1;
            y = 1;

            /*GESTION ZONE DE L'UTILISATEUR*/
            allUserBtn = new Button[4, 10];
            for (int j = 0; j < 10; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    userBtn = new Button(); //Déclaration dans la boucle sinon on se retrouve avec un bouton

                    this.userBtn.Click += new System.EventHandler(this.UserBtn_Click);
                    userBtn.Height = userBtn.Width = BUTTON_SIZE;
                    userBtn.Location = new System.Drawing.Point(x, y);
                    userBtn.Name = "userBtn" + indexUser;
                    x += BUTTON_SIZE + 10;
                    if (x >= (BUTTON_SIZE + 10) * 4)
                    {
                        x = 1;
                        y += BUTTON_SIZE + 10;
                    }

                    allUserBtn[k, j] = userBtn;
                    userPanel.Controls.Add(allUserBtn[k, j]);
                }
            }
            this.BackColor = Color.DarkSlateGray;
        }

        /*LIEN ENTRE ZONE DE CHOIX ET ZONE UTILISATEUR*/
        private void UserBtn_Click(object sender, EventArgs e) 
        {
            Button clickedButton = (Button)sender;
            roundControl++;

            if (indexUserY <= 9) //Limites nbre de lignes + Assignation d'une couleur
            {
                allUserBtn[indexUserX, indexUserY].BackColor = ((Button)sender).BackColor;
            }
            if (roundControl == 4)
            {
                Check();
                roundControl = 0; //Problème ici!
            }

            if (indexUserX >= 3) //Gestion couleurs d'une ligne (incrémentation et re-affectation)
            {
                indexUserX = 0;
                indexUserY++;
            }
            else
            {
                indexUserX++;
            }
            
            //SOLUTION IMPOSSIBLE: allUserBtn[k, j] = userBtn;
        }

        /*ZONE CONTRÔLE SAISIE UTILISATEUR*/
        public void Check()
        {          


            /*AUTRE MANIÈRE DE PARCOURIR UN TABLEAU: int i = 0;
             * foreach (Button check in solutionPanel.Controls.OfType<Button>())
             * {
                    if (check.BackColor == allUserBtn[i, indexUserY].BackColor)
                        MessageBox.Show("Bravo");

                    else
                        MessageBox.Show("Oh non!");

                    i++;
              *  }
            */

            //Contrôle saisie par rapport à la solution  
            for (int i = 0; i < choiceLenght; i++)
            {
                if (solutionPanel.Controls[i].BackColor != allUserBtn[i, indexUserY].BackColor) //Couleurs différentes
                {
                    int j = 0;
                    int check = 1;
                    bool isCorrect = true;
                    bool misplaced = false;
                    while (j < SOLUTION_LENGHT && misplaced == false || isCorrect == true)
                    {                       
                        if (solutionPanel.Controls[control].BackColor == allUserBtn[j, indexUserY].BackColor)
                        {
                            //colorPositionCheck.MisplacedColor();
                            misplaced = true;
                        }
                        else
                        {
                            isCorrect = false;
                        }
                        
                        try
                        {
                            foreach (Control frstItem in controlPanel.Controls)
                            {
                                if (frstItem.Name == "panel" + yCheck)
                                {
                                    Panel frst = frstItem as Panel;
                                    //frst.BackColor = Color.Black;
                                    foreach (Control sndItem in frst.Controls)
                                    {
                                        if (sndItem.Name == "controlBtn" + check + "Panel" + yCheck)
                                        {
                                            Button snd = sndItem as Button;
                                            /*Message contrôle position + condition désactivation boutons*/
                                            if (isCorrect)
                                            {
                                                if (misplaced)
                                                {
                                                    colorPositionCheck.MisplacedColor(snd);
                                                    MessageBox.Show("Mal placé");
                                                    //misplaced = false;
                                                }
                                                else
                                                {
                                                    endGame = true;
                                                    colorPositionCheck.WellplacedColor(snd);
                                                    //controlBtn.BackColor = Color.White;
                                                    EndMessage endMessageForm = new EndMessage();
                                                    endMessageForm.Show();
                                                    MessageBox.Show("Bravo");

                                                    /*PROBLÈME - DESACTIVATION D'UN BOUTON:
                                                     * for (int j = 0; j < choiceLenght; j++)
                                                       {
                                                           allChoiceBtn[j].Enabled = false;
                                                       }
                                                    */

                                                    foreach (Button button in choicePanel.Controls.OfType<Button>())
                                                    {
                                                        button.Enabled = false;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                colorPositionCheck.WrongColor(snd);
                                                MessageBox.Show("Oh non!");
                                                //isCorrect = true;
                                                //colorPositionCheck.WellplacedColor(allUserBtn[indexUserX, indexUserY]);
                                            }
                                            break;
                                        }
                                    }
                                    break;
                                }
                                else
                                {

                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                        j++;
                        xCheck++;
                    }
                    break;
                }
                control++;

                //colorPositionCheck.MisplacedColor(); //allUserBtn[j, indexUserY]
            }
            yCheck++;
        }

        private void restartBtn_Click(object sender, EventArgs e)
        {
            for (int j = 0; j < 10; j++)
            {
                for (int k = 0; k < 4; k++)
                {
                    allUserBtn[k, j].BackColor = Color.DarkSlateGray;
                    indexUserX = 0;
                    indexUserY = 0;
                }
            }
            if (endGame == true)
            {
                foreach (Button button in choicePanel.Controls.OfType<Button>())
                {
                    button.Enabled = true;
                }
                endGame = false;
            }
        }

        /*AUTRE SOLUTION:
         * Color currentColor = randomColors[m];
          string colorstring = currentcolor.tostring();

          switch (colorstring)
          {
              case "skyblue":
                  choiceBtn.Add("skyblue", Color.SkyBlue);
                  break;
              case "white":
                  choiceBtn.Add("white", Color.White);
                  break;
            default:
                break;
          }
         */


        private void firstFirstUsBtn_Click(object sender, EventArgs e)
        {
            firstFirstUsBtn.BackColor = ((Button)sender).BackColor;
        }

    }


    public class ColorPositionCheck
    {
        /*Fonction caractère bien placée*/

        enum CheckPosition
        {
            Wellpaced,
            Misplaced,
            Wrong
        }

        public void WellplacedColor(Button button)
        {
            SetColor(button, CheckPosition.Wellpaced);
        }

        public void MisplacedColor(Button button)
        {
            SetColor(button, CheckPosition.Misplaced);
        }

        public void WrongColor(Button button)
        {
            SetColor(button, CheckPosition.Wrong);
        }

        private void SetColor(Button button, CheckPosition position)
        {
            switch (position)
            {
                case CheckPosition.Wellpaced:
                    button.BackColor = Color.White;
                    break;
                case CheckPosition.Misplaced:
                    button.BackColor = Color.Gray;
                    break;
                case CheckPosition.Wrong:
                    button.BackColor = Color.Black;
                    break;
            }
        }

    }
}
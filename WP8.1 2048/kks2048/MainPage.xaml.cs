using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;



namespace kks2048
{
    public sealed partial class MainPage : Page
    {

        private Point currentPoint;
        private Point oldPoint;
        private double distanceX;
        private double distanceY;
       
        public int[,] Block = new int[4, 4];
        public bool[,] HasConflicted = new bool[4, 4];

        //建立一个4*4的数组，来存储数字




        Scores scores = new Scores() ;
        
        public MainPage()
        {
            this.InitializeComponent();

            textScore.DataContext = scores;
            this.NavigationCacheMode = NavigationCacheMode.Required;
        }
        private void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    Block[i,j] = 0;
                    HasConflicted[i, j] = false;
                }
            }
            

            scores.Score = 0;
        }

        private void updateBoardView()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j <4; j++)
                {
                    if (Block[i,j]==0)
                    {
                        DeleteButton(i, j);
                    }
                    else
                    {
                        AddButton(i, j, Block[i, j]);
                    }
                    HasConflicted[i,j] = false;
                }
            }
        }
        private void NewNum()
        {
            Random random = new Random();
            int num = random.Next(0, 9) > 4 ? 2 : 4;//随机生成2、4

            int nullnum = 0;//剩余空格数，随机确定生成位置
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (Block[i, j] == 0)
                        nullnum++;
            if (nullnum < 1)
            {
                return;
            }

            int index = random.Next(1, nullnum);//随机产生一个位置
            nullnum = 0;
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 4; j++)
                    if (Block[i, j] == 0)
                    {
                        nullnum++;
                        if (nullnum != index) continue;
                        Block[i, j] = num;//将数字放到随机产生的位置
                        AddButton(i, j, Block[i, j]);
                     }
        }

        private void AddButton(int i, int j, int num)
        {
            Button btn = (Button)mainGrid.Children
                .Cast<FrameworkElement>()
                .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
             btn.Content = num.ToString();
             btn.Background = new SolidColorBrush(getNumberBackgroundColor(num));  
           
            
            //btn.Visibility = Visibility.Visible;
            //storyboad = new Storyboard();
            //DoubleAnimation opacity = new DoubleAnimation();
            //opacity.From = 0;
            //opacity.To = 1;
            //opacity.Duration = TimeSpan.FromMilliseconds(500);
            //Storyboard.SetTarget(opacity, btn);
            //Storyboard.SetTargetProperty(opacity, "Opacity");
            //storyboad.Begin();

        }
        private void NewGame()
        {
            Init();
            NewNum();
            NewNum();
            updateBoardView();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Init();
            NewNum();
            NewNum();
        }

        private Color getNumberBackgroundColor(int number)
        {
            Color backColor;
            switch (number)
            {
                case 2:
                    backColor = Colors.LightPink;
                    break;
                case 4:
                    backColor = Colors.LightSalmon;
                    break;
                case 8:
                    backColor = Colors.Tomato;
                    break;
                case 16:
                    backColor = Colors.Violet;
                    break;
                case 32:
                    backColor = Colors.HotPink;
                    break;
                case 64:
                    backColor = Colors.DarkOrchid;
                    break;
                case 128:
                    backColor = Colors.Magenta;
                    break;
                case 256:
                    backColor = Colors.MediumVioletRed;
                    break;
                case 512:
                    backColor = Colors.PaleGreen;
                    break;
                case 1024:
                    backColor = Colors.Lime;
                    break;
                case 2048:
                    backColor = Colors.LightSkyBlue;
                    break;
                case 4096:
                    backColor = Colors.RoyalBlue;
                    break;
                default:
                    backColor = Colors.DarkSalmon;
                    break;
            }
            return backColor;
        }
        private bool MoveUp()
        {
            if (!CanMoveUp())
            {
                IsGameOver();
                return false;
            }
            for (int j = 0; j <4; j++)
            {
                for (int i = 1; i <4; i++)
                {
                    if (Block[i,j]!=0)
                    {
                        for (int k = 0; k <i; k++)
                        {
                            if (Block[k, j] == 0 && noBlockVertical(j, k, i))
                            {
                                Block[k, j] = Block[i, j];
                                Block[i, j] = 0;
                                continue;
                          
                            }
                            else if (Block[k, j] == Block[i, j] && noBlockVertical(j, k, i)&&!HasConflicted[k,j])
                            {
                                Block[k, j] += Block[i, j];
                              
                               
                                scores.Score += Block[i, j];
                                Block[i, j] = 0;
                                HasConflicted[k, j] = true;
                                continue;
                            }
                        }
                    }
                }
            }
            updateBoardView();
            NewNum();
            return true;
        }

        private bool noBlockVertical(int col, int row1, int row2)
        {
            for (int i = row1 + 1; i < row2; i++)
            {
                if (Block[i, col] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private bool MoveDown()
        {
            if (!CanMoveDown())
            {
                IsGameOver();
                return false;
            }
            for (int j = 0; j <4; j++)
            {
                for (int i= 2; i >=0; i--)
                {
                    if (Block[i,j]!=0)
                    {
                        for (int k = 3; k >i; k--)
                        {
                            if (Block[k, j] == 0  && noBlockVertical(j, i, k))
                            {
                                Block[k, j] = Block[i, j];
                                Block[i, j] = 0;
                                continue;
                            }
                            else if (Block[k, j] == Block[i, j] && noBlockVertical(j, i, k)&&!HasConflicted[k,j])
                            {
                                Block[k, j] += Block[i, j];
                               
                                
                                scores.Score += Block[i, j];
                                Block[i, j] = 0;
                                HasConflicted[k, j] = true;
                                continue;
                              
                            }
                        }
                    }
                }
            }
            updateBoardView();
            NewNum();
            return true;
        }

        private bool MoveLeft()
        {
            if (!CanMoveLeft())
            {
                IsGameOver();
                return false;
            }
            for (int i = 0; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {
                    if (Block[i, j] != 0)
                    {
                        for (int k = 0; k < j; k++)
                        {
                            if (Block[i, k] == 0  &&noBlockHorizontal(i, k, j))
                            {
                                
                                Block[i, k] = Block[i, j];
                                Block[i, j] = 0;
                               
                                continue;
                            }
                            else if
                            (Block[i, k] == Block[i, j] &&  noBlockHorizontal(i, k, j)&&!HasConflicted[i,k] )
                            {
                                
                               Block[i, k] += Block[i, j];
                               
                               scores.Score += Block[i, j];
                               Block[i, j] = 0;
                               HasConflicted[i, k] = true;
                               continue;
                            }
                        }
                    }
                }
            updateBoardView();
            NewNum();
            return true;
        }


        private bool MoveRight()
        {
            if (!CanMoveRight())
            {
                IsGameOver();
                return false;
            }
                
            for (int i = 0; i <4; i++)
            
                for (int j = 2; j >=0 ; j--)
                {
                    if (Block[i,j]!=0)
                    {
                        for (int k = 3; k>j; k--)
                        {
                            if (Block[i, k] == 0 && noBlockHorizontal(i, j, k))
                            {
                                
                                Block[i, k] = Block[i, j];
                                Block[i, j] = 0;
                               

                              
                                continue;
                            }
                            else if(Block[i, k] == Block[i, j]  && noBlockHorizontal(i, j, k)&&!HasConflicted[i,k])
                            {
                                
                                Block[i, k] += Block[i, j];
                                
                                scores.Score += Block[i, j];
                                Block[i, j] = 0;
                                HasConflicted[i, k] = true;
                               

                                continue;
                            }
                        }
                    }
                }
            
            updateBoardView();
            NewNum();
            return true;
        }
        private bool noBlockHorizontal(int row, int col1, int col2)
        {
            for (int i = col1 + 1; i < col2; i++)
            {
                if (Block[row, i] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        private void DeleteButton(int i, int j)
        {
            Button btn = (Button)mainGrid.Children
               .Cast<FrameworkElement>()
               .First(e => Grid.GetRow(e) == i && Grid.GetColumn(e) == j);
            btn.Content = null;
            btn.Background = new SolidColorBrush(Colors.LightPink);
         }

      
        public bool CanMoveLeft()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 1; j < 4; j++)
                {
                    if (Block[i,j] != 0)
                    {
                        if (Block[i,j-1] == 0 || Block[i,j-1] == Block[i,j])
                            return true;
                    }
                }
            return false;
        }
        public bool CanMoveRight()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 2; j >= 0; j--)
                {
                    if (Block[i, j] != 0)
                        if (Block[i, j+1] == 0 || Block[i,j+1] == Block[i, j])
                        {
                            return true;
                        }
                }
            }
            return false;
        }

        public bool CanMoveUp()
        {
            for (int j = 0; j < 4; j++)
                for (int i = 1; i < 4; i++)
                {
                    if (Block[i, j] != 0)
                        if (Block[i - 1, j] == 0 || Block[i - 1, j] == Block[i, j])
                            return true;
                }
            return false;
        }
        public bool CanMoveDown()
        {
            for (int j = 0; j < 4; j++)
                for (int i = 2; i >= 0; i--)
                {
                    if (Block[i, j] != 0)
                        if (Block[i + 1, j] == 0 || Block[i + 1, j] == Block[i, j])
                            return true;
                }
            return false;
        }

          private bool NoSpace()//格子是否填满
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Block[i, j] == 0)
                        return false;
                }
            }
            return true;
        }
        
        private bool CanMove()
        {
            if (CanMoveDown() || CanMoveUp() || CanMoveLeft() || CanMoveRight())
                return false;
            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

       private void mainGrid_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            

            distanceX = e.Cumulative.Translation.X;
            distanceY = e.Cumulative.Translation.Y;
            e.Complete();
            if (Math.Abs(distanceX) < 5 && Math.Abs(distanceY) < 5)
                return;
            if (Math.Abs(distanceX) > Math.Abs(distanceY))
            {
                if (distanceX > 0)
                    MoveRight();
             
                else
                    MoveLeft();
                   
            }
            else
            {
                if (distanceY > 0)
                  MoveDown();
                  
                else
                    MoveUp();
                   
            }
           
        }

        private  void IsGameOver()
        {
            if (NoSpace()&&CanMove())
            {
                GameOver();

            }
        }

        private async void GameOver()
        {
            MessageDialog ms = new MessageDialog("you lost!");
            await ms.ShowAsync();

        }

      
    }
}

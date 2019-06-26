using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Robocode;
using Robocode.Util;

namespace Projeto_Robo_Solutis
{
    class MeuRobo : AdvancedRobot
    {
        double energiaInimigo;
        double energiaPessoal;
        //Alvo da nossa fúria
        Boolean alvoEncontrado = false;
        //Modo a ser ativado quando a energia ficar abaixo de 30.
        Boolean modoDeProtecao = false;
        public override void Run()
        {
            energiaPessoal = Energy;
            if(energiaPessoal < 30)
            {
                modoDeProtecao = true;
            }
            else
            {
                modoDeProtecao = false;
            }
            //Define as cores do robô.
            SetColors(Color.Green, Color.Brown, Color.Crimson);
            Random ale = new Random();
            if (alvoEncontrado)
            {
                Ahead(10);
                alvoEncontrado = false;
            }
            if(!modoDeProtecao && !alvoEncontrado)
            {
                Out.Write('A');
                while (!modoDeProtecao)
                {
                    int res = ale.Next(1);
                    //Out.Write(res);
                    Out.WriteLine();

                    switch (res)
                    {
                        case 0:
                            SetAhead(50);
                            SetTurnLeft(45);
                            Execute();
                            //SetTurnGunLeft(90);
                            break;

                        case 1:
                            SetAhead(100);
                            SetTurnRight(90);
                            Execute();
                            //SetTurnGunRight(90);
                            break;
                    }
                }
            }
            if(modoDeProtecao)
            {
                Ahead(200);
                
            }


        }

        public override void OnScannedRobot(ScannedRobotEvent e)
        {
            energiaInimigo = e.Energy;
            if (e.Distance > 500 || modoDeProtecao )
            {
                Fire(1);
                //Muito longe/perigoso, deixa pra lá!
            }
            else
            {
                alvoEncontrado = true;
                TurnRight(e.Bearing);
                SetAhead(e.Distance);
                if (energiaInimigo < 30)
                {
                    Fire(energiaInimigo);
                }
                else
                {
                    Fire(5);
                }
                Execute();
            }
        }

        public override void OnBulletHit(BulletHitEvent evnt)
        {

        }
        public override void OnHitByBullet(HitByBulletEvent evnt)
        {
            if(energiaPessoal < 30)
            {
                //Executar modelo de proteção!
                SetTurnRight(-evnt.Bearing);
                Back(150);
                modoDeProtecao = true;
            }
            else
            {
                modoDeProtecao = false;
            Back(5);
            }   
        }

        public override void OnHitWall(HitWallEvent evnt)
        {
            if(modoDeProtecao)
            {
                //Girar para direita
                if(evnt.Bearing > 0) {
                    TurnRight(90);
                }
                else
                {
                    TurnLeft(90);
                }
            }else
            {
            SetTurnRight(evnt.Bearing);
            Back(100);
            }
        }
        public override void OnRobotDeath(RobotDeathEvent evnt)
        {
            //Triste demais :(
        }
        public override void OnSkippedTurn(SkippedTurnEvent evnt)
        {
            Back(200);
        }

        public override void OnWin(WinEvent evnt)
        {
            //Dança da vitória!!!
            TurnRight(720);
        }
    }
}
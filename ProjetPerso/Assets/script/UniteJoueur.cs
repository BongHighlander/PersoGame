using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniteJoueur : MonoBehaviour
{
    public int pdv = 5;
    public int mouvements = 5;
    protected Vector3 direction = Direction.OUEST;
    Dictionary<Vector3, int> listeMouvements;
    Queue<Vector3> tendance = new Queue<Vector3>();

    public void Initialiser(Carte carte, Vector3 positionIA)
    {
        mouvements = 7;
        listeMouvements = new Dictionary<Vector3, int>();
        RemplirListe(mouvements, transform.position, direction, carte, positionIA);
        listeMouvements.Remove(transform.position);
        listeMouvements.Add(transform.position, mouvements);
    }

    internal Dictionary<Vector3, int> DemanderListeMouvements()
    {
        return listeMouvements;
    }



    private void RemplirListe(int deplacementRestant, Vector3 actuel, Vector3 directionTemp, Carte carte, Vector3 positionIA)
    {
        if (carte.GetType(actuel) != TypeTerrains.BATIMENT)
        {
            Direction.VOISINS.ForEach(delegate (Vector3 voisin)
            {
                if (carte.GetType(actuel + voisin) != TypeTerrains.BATIMENT || (actuel+voisin!=positionIA))
                {
                    int reste;

                    if (voisin == directionTemp)
                    {
                        reste = deplacementRestant - 1;
                    }
                    else if (voisin == -directionTemp)
                    {
                        reste = deplacementRestant - 3;
                    }
                    else
                    {
                        reste = deplacementRestant - 2;
                    }

                    if (!listeMouvements.ContainsKey(actuel + voisin))
                    {
                        if (reste > -1)
                        {
                            listeMouvements.Add(actuel + voisin, reste);
                            RemplirListe(reste, actuel + voisin, voisin, carte, positionIA);
                        }
                    }
                    else if (reste > listeMouvements[actuel + voisin])
                    {
                        listeMouvements[actuel + voisin] = reste;
                        RemplirListe(reste, actuel + voisin, voisin, carte, positionIA);
                    }
                }
            });
        }
    }

    public Vector3 remplirListPath(Path p)
    {
        listeMouvements = new Dictionary<Vector3, int>();
        Vector3 res = new Vector3(0, 0, 0);
        int i = 1;
        while (p.previousStep != null)
        {
            listeMouvements.Add(p.lastStep, i);
            if(i == mouvements)
            {
                res = p.lastStep;
            }
            i++;
            p = p.previousStep;

        }
        // on prend la fin de la list chainée
        listeMouvements.Add(p.lastStep, i);
        if (i == mouvements)
        {
            res = p.lastStep;
        }
        // Si le chemin plus petit que mouvement
        if(res==new Vector3(0, 0, 0))
        {
            res = p.lastStep;
        }
        return res;
    }

    public Stack<Vector3> Chemin(Vector3 position)
    {
        Stack<Vector3> reponse = new Stack<Vector3>();
        reponse.Push(position);

        if (!listeMouvements.ContainsKey(position))
        {
            return null;
        }
        else
        {
            while (reponse.Peek() != transform.position)
            {
                int size = reponse.Count;

                int indice = listeMouvements[reponse.Peek()] + 6;
                while (reponse.Count == size && indice > listeMouvements[reponse.Peek()])
                {
                    indice--;
                    Direction.VOISINS.ForEach(delegate (Vector3 voisin)
                    {
                        try 
                        {
                            if (listeMouvements[reponse.Peek() + voisin] == indice)
                            {
                                reponse.Push(reponse.Peek() + voisin);
                                indice = -1;
                            }
                        }
                        catch
                        {

                        }
                    });
                }
            }
        }

        return reponse;
    }

    internal void RotationTourelle(Vector3 position)
    {
        GameObject tourelle = transform.Find("Tourelle").gameObject;
        tourelle.transform.LookAt(position);


    }

    private void Update()
    {
        if (tendance.Count != 0)
        {
            var targetRotation = Quaternion.LookRotation(tendance.Peek() - transform.position);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime);


            transform.position = Vector3.MoveTowards(transform.position, tendance.Peek(), Time.deltaTime);
            if (transform.position == tendance.Peek())
            {
                tendance.Dequeue();
            }
        }

    }

    public void Deplacement(Vector3 position)
    {
        Stack<Vector3> chemin = Chemin(position);
        if (chemin == null)
        {
            print("pb dans list mov");
        }
        else
        {
            while (chemin.Count > 1)
            {
                Vector3 suite = chemin.Pop() - chemin.Peek();
                if (suite == -direction)
                {
                    mouvements--;
                    mouvements--;
                }
                else if (suite != direction)
                {
                    mouvements--;
                }
                mouvements--;
                tendance.Enqueue(chemin.Peek());
            }
        }
    }


    public void DeplacementDroit(Vector3 position)
    {
        mouvements -= (int)Math.Abs(transform.position.x - position.x);
        mouvements -= (int)Math.Abs(transform.position.z - position.z);
        tendance.Enqueue(position);
    }

    public void OrdreGauche()
    {
        mouvements--;
        for (int i = 0; i < 9; i++)
        {
            transform.Rotate(Vector3.up * 10);
        }
    }

    public void OrdreDroite()
    {
        mouvements--;
        for (int i = 0; i < 9; i++)
        {
            transform.Rotate(Vector3.up * -10);
        }
    }

    public void OrdreDemiTour()
    {
        mouvements--;
        mouvements--;
        for (int i = 0; i < 18; i++)
        {
            transform.Rotate(Vector3.up * 10);
        }
    }
}

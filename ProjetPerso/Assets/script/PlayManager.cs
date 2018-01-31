using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayManager : MonoBehaviour
{

    public static PlayManager instance = null;
    public Carte carte;

    public GameObject[] teamPlayer;
    public GameObject[] teamEnemy;
    
    // missile
    public GameObject m_missile;
    public Transform missileSpawn;


    private GameObject missile = null;
    private Vector3 targetMissile;
    private bool statusMissile = false;

    private int mouvementPlayer = 0;
    private Vector3 currentMove = new Vector3(4, 0, 0);
    public Camera camera;
    public bool tourIA =false;

    public static PlayManager getInstance()
    {
        return instance;
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        teamPlayer[0] = Instantiate(teamPlayer[0], new Vector3(4, 0, 0), Quaternion.identity) as GameObject;
        teamEnemy[0] = Instantiate(teamEnemy[0], new Vector3(0, 0, 9), Quaternion.identity) as GameObject;
        teamEnemy[0].GetComponent<IAenemy>().InitialiserIA(this);
        TurnPlayer();
    }

    private void Update()
    {

        teamEnemy[0].GetComponent<IAenemy>().tracePath();
        int dep = 2;
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Vector3 dest = new Vector3(camera.transform.position.x - dep, camera.transform.position.y, camera.transform.position.z);
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, dest, 10 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Vector3 dest = new Vector3(camera.transform.position.x + dep, camera.transform.position.y, camera.transform.position.z);
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, dest, 10 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            Vector3 dest = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z - dep);
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, dest, 10 * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Vector3 dest = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + dep);
            camera.transform.position = Vector3.MoveTowards(camera.transform.position, dest, 10 * Time.deltaTime);
        }

        if (missile != null)
        {
            deplacementMissile();
        }

        if (mouvementPlayer > 0)
        {
            teamPlayer[0].GetComponent<UniteJoueur>().Initialiser(carte, teamEnemy[0].transform.position);
            EteindreCases();
            EclairerCases(teamPlayer[0]);
            if (teamPlayer[0].GetComponent<UniteJoueur>().DemanderListeMouvements().ContainsKey(currentMove))
            {
                MontrerChemin(teamPlayer[0], currentMove);
            }
        }
    }


    public void TurnPlayer()
    {
        mouvementPlayer++; //ensuite on attend le reste du tour des clics dans la fonction ClicCase
    }

    private void EclairerCases(GameObject acteur)
    {
        foreach (KeyValuePair<Vector3, int> entry in acteur.GetComponent<UniteJoueur>().DemanderListeMouvements())
        {
            try
            {
                carte.GetTile(entry.Key).GetComponent<Case>().Allumer(Color.Lerp(Color.yellow, Color.green, (float)entry.Value / (float)acteur.GetComponent<UniteJoueur>().mouvements));
            }
            catch
            {

            }
        }
    }

    internal void ClicCase(Vector3 position)
    {
        EteindreCases();
        //ClicShoot(position);

        if (mouvementPlayer == 1) //premier clic
        {
            currentMove = position;
            mouvementPlayer++;
        }
        else if (mouvementPlayer == 2 && currentMove == position) //re clic pour confirmer deplacement
        {
            teamPlayer[0].GetComponent<UniteJoueur>().Deplacement(position);
            IAturn();
            mouvementPlayer--;
            mouvementPlayer--;
        }
        else if (mouvementPlayer == 2) //clic ailleurs pour annuler
        {
            mouvementPlayer--;
            EclairerCases(teamPlayer[0]);
        }
    }

    internal void ClicShoot(Vector3 position)
    {
        if (!statusMissile)
        {
            teamPlayer[0].GetComponent<UniteJoueur>().RotationTourelle(teamEnemy[0].transform.position);
            missile = (GameObject)Instantiate(
                m_missile,
                teamPlayer[0].transform.position,
                missileSpawn.rotation);
            statusMissile = true;
            targetMissile = position;
            missile.transform.LookAt(targetMissile);
        }

    }

    public void deplacementMissile()
    {
        if (statusMissile)
        {
            if (!carte.IsInRange(missile.transform.position)) {
                Destroy(missile);
                statusMissile = false;
            } else {
                missile.transform.position = Vector3.MoveTowards(missile.transform.position, targetMissile, 10 * Time.deltaTime);
                if (missile.transform.position == targetMissile)
                {   
                    Destroy(missile);
                    statusMissile = false;
                    if (targetMissile == teamEnemy[0].transform.position)
                    {
                        teamEnemy[0].GetComponent<IAenemy>().pdv--;
                    }
                    else if((targetMissile == teamPlayer[0].transform.position))
                    {
                        teamEnemy[0].GetComponent<UniteJoueur>().pdv--;
                    }
                    if (tourIA)
                    {
                        TurnPlayer();
                    }
                    else
                    {
                        IAturn();
                    }
                }
            }
            
        }
    }


    private void EteindreCases()
    {
        foreach (KeyValuePair<Vector3, GameObject> entry in carte.listRealTiles)
        {
            try
            {
                entry.Value.GetComponent<Case>().Eteindre();
            }
            catch
            {

            }
        }
    }

    private void MontrerChemin(GameObject acteur, Vector3 position)
    {
        Stack<Vector3> chemin = acteur.GetComponent<UniteJoueur>().Chemin(position);
        while (chemin.Count != 0)
        {
            carte.GetTile(chemin.Pop()).GetComponent<Case>().Allumer(Color.cyan);
        }
    }

    private void IAturn()
    {
        tourIA = true;
        teamEnemy[0].GetComponent<IAenemy>().Initialiser(carte, teamPlayer[0].transform.position);
        BougerIA();
        statusMissile = false;
        TirerIA(teamPlayer[0].transform.position);
    }


    /// <summary>
    /// Ici ce sera à peu près la que devra se trouver l'A* pour séléctionner le "meilleur" mouvement pour l'IA
    /// On attrappe le type batiment en faisant carte.GetType(vecteur de la position demandée);
    /// Penser à empêcher l'ennemi de sortir de la map
    /// </summary>
    private void BougerIA()
    {
        

        Path p = teamEnemy[0].GetComponent<IAenemy>().FindPath(teamEnemy[0].transform.position, teamPlayer[0].transform.position, carte);
        Vector3 res = new Vector3(0, 0, 0);
        teamEnemy[0].GetComponent<IAenemy>().tracePath(p);
        Vector3 chosen = teamEnemy[0].GetComponent<UniteJoueur>().remplirListPath(p);
        teamEnemy[0].GetComponent<IAenemy>().Deplacement(chosen);
    }
    private void TirerIA(Vector3 cible)
    {
        if (!statusMissile)
        {
            missile = (GameObject)Instantiate(
                m_missile,
                teamEnemy[0].transform.position,
                missileSpawn.rotation);
            statusMissile = true;
            targetMissile = cible;
            missile.transform.LookAt(targetMissile);
        }
    }


}




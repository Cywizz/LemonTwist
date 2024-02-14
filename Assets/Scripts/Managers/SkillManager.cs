using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkillManager : MonoBehaviour
{
    [HideInInspector]
    public LemonSkillsEnum SelectedSkill = LemonSkillsEnum.None;
    
    [Header("Skill variables")]
    public int BuilderSecondsToBuild;   
    public Tile BuilderPlatformTile;
    public int DiggerSecondsToDig;


    
   


    public static SkillManager Instance { get; private set; }

    #region Unity Events

    private void Awake()
    {
        #region Singleton Setup
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion

        
        

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            SelectedSkill = LemonSkillsEnum.Blocker;
        }

        if(Input.GetKeyDown(KeyCode.H))
        {
            SelectedSkill = LemonSkillsEnum.Builder;
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            SelectedSkill = LemonSkillsEnum.Juicer;
        }

        if(Input.GetKeyDown(KeyCode.D))
        {
            SelectedSkill = LemonSkillsEnum.Digger;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SelectedSkill = LemonSkillsEnum.None;
        }



    }

    #endregion

    #region Public Members

    public void Dig(LemonGameController lemonController)
    {
        var go = GameObject.Find("Ground");
        Tilemap groundTileMap = go.GetComponent<Tilemap>();

        var cellLemonOnPos = groundTileMap.WorldToCell(lemonController.transform.position);
        //Note: might need to adjust the lemon's position here to be in centre of tile
        
        

        StartCoroutine(StartDigging(cellLemonOnPos, lemonController));

    }

    public void BuildPlatform(LemonGameController lemonController)
    {
        //need to see if there is an gap in the ground to dig on either side, otherwise we need to set the skill back to none

        var go = GameObject.Find("Ground");
        Tilemap groundTileMap = go.GetComponent<Tilemap>();

        var cellPosLemonOn = groundTileMap.WorldToCell(lemonController.transform.position);
        var tileLemonOn = groundTileMap.GetTile(cellPosLemonOn);

        Vector3Int cellToBuildPos = Vector3Int.zero;
        var tileRightNeighbourPos = new Vector3Int(cellPosLemonOn.x + 1, cellPosLemonOn.y);
        var tileRightNeigbour = groundTileMap.GetTile(tileRightNeighbourPos);
        if(tileRightNeigbour == null)
        {
            //build to the right of lemon
            cellToBuildPos = tileRightNeighbourPos;

        }
        else
        {
            //try left neigbour
            var tileLeftNeighbourPos = new Vector3Int(cellPosLemonOn.x -1, cellPosLemonOn.y);
            var tileLeftNeigbour = groundTileMap.GetTile(tileLeftNeighbourPos);
            
            if(tileLeftNeigbour == null)
            {
                //build to the right of the lemon
                cellToBuildPos = tileLeftNeighbourPos;
            }          
        }

        if (cellToBuildPos != Vector3Int.zero)
        {
            StartCoroutine(StartBuilding(cellToBuildPos, lemonController));
        }
        else
        {
            //nothing to build, change skill to non
            lemonController.SetSkill(LemonSkillsEnum.None);
        }       

        
    }

    #endregion

    #region Private Members

    private IEnumerator StartDigging(Vector3Int tilePos, LemonGameController lemonController)
    {
        yield return new WaitForSeconds(SkillManager.Instance.DiggerSecondsToDig);

        var go = GameObject.Find("Ground");
        Tilemap groundTileMap = go.GetComponent<Tilemap>();

        groundTileMap.SetTile(tilePos, null);
        groundTileMap.SetColliderType(tilePos, Tile.ColliderType.None);

        //done digging, move on
        lemonController.SetSkill(LemonSkillsEnum.None);
    }

    private IEnumerator StartBuilding(Vector3Int tilePos, LemonGameController lemonController)
    {
        yield return new WaitForSeconds(SkillManager.Instance.BuilderSecondsToBuild);

        var go = GameObject.Find("Ground");
        Tilemap groundTileMap = go.GetComponent<Tilemap>();

        groundTileMap.SetTile(tilePos, BuilderPlatformTile);

        groundTileMap.SetColliderType(tilePos, Tile.ColliderType.Sprite);

        //done building, move on
        lemonController.SetSkill(LemonSkillsEnum.None);
    }

    #endregion

}

public enum LemonSkillsEnum
{
    None,
    Blocker,
    Builder,
    Juicer,
    Digger
}

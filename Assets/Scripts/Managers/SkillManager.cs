using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SkillManager : MonoBehaviour
{
    [HideInInspector]
    private LemonSkillsEnum _selectedSkill = LemonSkillsEnum.None;

    [Header("Skill variables")]
    public int BuilderSecondsToBuild;   
    public Tile BuilderPlatformTile;
    public int DiggerSecondsToDig;
    public int BasherSecondsToBash;

    public Dictionary<LemonGameController, Vector3Int> _crumblingDictionary = new Dictionary<LemonGameController, Vector3Int>();
    
   


    public static SkillManager Instance { get; private set; }
    public LemonSkillsEnum SelectedSkill
    {
        get
        {
            return _selectedSkill;
        }
        set
        {
            if(value != _selectedSkill) AudioManager.Instance.PlaySFX(SFXSoundsEnum.ButtonPop);

            _selectedSkill = value;
        }
    }

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

        if(Input.GetKeyDown(KeyCode.L))
        {
            SelectedSkill = LemonSkillsEnum.Basher;
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SelectedSkill = LemonSkillsEnum.None;
        }

       



    }

    #endregion

    #region Public Members

    public void Bash(LemonGameController lemonController)
    {
        var go = GameObject.Find("Ground");
        var bounceGO = GameObject.Find("Bounce");
        Tilemap groundTileMap = go.GetComponent<Tilemap>();
        Tilemap bounceTileMap = bounceGO.GetComponent<Tilemap>();

        var cellLemonOnPos = groundTileMap.WorldToCell(lemonController.transform.position);

        //there must be a tile either left or right of the lemon to proceed
        TileBase tileToBash = null;
        Vector3Int tileToBashPos = Vector3Int.zero;

        var testTileLeft = bounceTileMap.GetTile(cellLemonOnPos + new Vector3Int(-1, 1, cellLemonOnPos.z));
        if(testTileLeft == null)
        {//try right
            var testTileRight = bounceTileMap.GetTile(cellLemonOnPos + new Vector3Int(1, 1, cellLemonOnPos.z));
            if (testTileRight != null)
            {
                tileToBash = testTileRight;
                tileToBashPos = cellLemonOnPos + new Vector3Int(1, 1, cellLemonOnPos.z);
            }
        }
        else
        {
            tileToBash = testTileLeft;
            tileToBashPos = cellLemonOnPos + new Vector3Int(-1, 1, cellLemonOnPos.z);
        }

        if(tileToBash == null)
        {
            //nothing to bash, change skill to non
            lemonController.SetSkill(LemonSkillsEnum.None);
        }
        else
        {
            StartCoroutine(StartBashing(tileToBashPos, groundTileMap, bounceTileMap, lemonController));
        }


    }

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

    public void LemonOverCrumbleTile(LemonGameController lemonController)
    {
        var groundGO = GameObject.Find("Ground");
        var crumbleGO = GameObject.Find("Crumble");

        Tilemap crumbleTileMap = crumbleGO.GetComponent<Tilemap>();
        Tilemap groundTileMap = groundGO.GetComponent<Tilemap>();

        var cellPosLemonOn = crumbleTileMap.WorldToCell(lemonController.transform.position);

        var testTile = crumbleTileMap.GetTile(cellPosLemonOn);
        if(testTile == null)
        {
            //it might be that the lemon was too fast and it registered a neighbour, so try left and right
            testTile = crumbleTileMap.GetTile(cellPosLemonOn + new Vector3Int(1,0,0));
            if(testTile == null)
            {
                testTile = crumbleTileMap.GetTile(cellPosLemonOn - new Vector3Int(1, 0, 0));

                if(testTile != null) cellPosLemonOn = cellPosLemonOn - new Vector3Int(1, 0, 0);
            }
            else
            {
                cellPosLemonOn = cellPosLemonOn + new Vector3Int(1, 0, 0);
            }
        }
        

        if (!_crumblingDictionary.ContainsKey(lemonController))
        {
            _crumblingDictionary.Add(lemonController, cellPosLemonOn);            

            StartCoroutine(StartTileCrumbling(cellPosLemonOn, groundTileMap, crumbleTileMap, lemonController));
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

    private IEnumerator StartTileCrumbling(Vector3Int tilePos, Tilemap groundTM, Tilemap crumbleTM, LemonGameController lemonController)
    {
        yield return new WaitForSeconds(1f);


        crumbleTM.SetTile(tilePos, null);
        crumbleTM.SetColliderType(tilePos, Tile.ColliderType.None);

        groundTM.SetTile(tilePos, null);
        groundTM.SetColliderType(tilePos, Tile.ColliderType.None);

        _crumblingDictionary.Remove(lemonController);

    }

    private IEnumerator StartBashing(Vector3Int tilePos, Tilemap groundTM, Tilemap bounceTM, LemonGameController lemonController)
    {
        yield return new WaitForSeconds(SkillManager.Instance.BasherSecondsToBash);

        groundTM.SetTile(tilePos, null);
        groundTM.SetColliderType(tilePos, Tile.ColliderType.None);

        bounceTM.SetTile(tilePos, null);
        bounceTM.SetColliderType(tilePos, Tile.ColliderType.None);

        //done bashing, move on
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
    Digger,
    Basher
}

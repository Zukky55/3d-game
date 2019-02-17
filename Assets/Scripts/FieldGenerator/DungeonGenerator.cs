#if true
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ReviewGames
{
    public class DungeonGenerator : MonoBehaviour
    {
        /// <summary>スタート地点</summary>
        public Cell StartCell { get; private set; }
        /// <summary>ゴール地点</summary>
        public Cell GoalCell { get; private set; }
        /// <summary>全ての道Cell</summary>
        public List<Cell> RoadCells { get; private set; }

        /// <summary>DungeonMapのオブジェクト群</summary>
        List<GameObject> m_dungeonMapObjects;
        /// <summary>DungeonMapのコンポーネント群</summary>
        List<Cell> m_dungeonMapModules;
        /// <summary>道の偶数地点</summary>
        List<Cell> m_roadEvenCell;
        /// <summary>行き止まり地点</summary>
        List<Cell> m_noPassageCells;
        /// <summary>Goal迄の最短通路</summary>
        List<Cell> m_roadToGoal;
        /// <summary>StateMachine</summary>
        StateManager m_stateManager;


        /// <summary>迷路の外周分の余白</summary>
        const int m_margin = 2;
        /// <summary>ターゲットにする範囲</summary>
        const int targetRange = 2;
        /// <summary>サブターゲットにする範囲</summary>
        const int subTargetRange = 1;

        /// <summary>ゴール地点を設定しているかどうか</summary>
        bool m_isGoalSet;

        ///// <summary>goal portal</summary>
        //[SerializeField] GameObject m_goalPortalPrefab;
        [Header("Modules")]
        /// <summary>portal</summary>
        [SerializeField] GameObject m_portalPrefab;
        /// <summary>coin</summary>
        [SerializeField] GameObject m_coinPrefab;
        /// <summary>gravity ball</summary>
        [SerializeField] GameObject m_gravityBallPrefab;
        /// <summary>player</summary>
        [SerializeField] GameObject m_player;
        /// <summary>幅</summary>
        [SerializeField] int m_width;
        /// <summary>高さ</summary>
        [SerializeField] int m_height;
        /// <summary>奥行</summary>
        [SerializeField] int m_depth;

        /// <summary>map size</summary>
        [Header("Can be changed during execution.")]
        [SerializeField] float m_mapSize = 1f;

        /// <summary>DungeonMapの透明度</summary>
        [Header("Can't be changed during execution.")]
        [Range(0f, 1f)] [SerializeField] public float m_alpha = 1f;

        private void Awake()
        {
            m_dungeonMapObjects = new List<GameObject>();
            m_dungeonMapModules = new List<Cell>();
            m_roadEvenCell = new List<Cell>();
            m_noPassageCells = new List<Cell>();
            m_roadToGoal = new List<Cell>();
            RoadCells = new List<Cell>();

            Setup();
        }

        /// <summary>
        /// Inspectorで値を変更した時に呼ばれるCallback method
        /// </summary>
        private void OnValidate()
        {
            // Play中以外スルー
            if (!Application.isPlaying)
            {
                return;
            }
            // mapSizeに合わせてScale変更
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);
        }

        /// <summary>
        /// Set the Dungeon map
        /// </summary>
        public void Setup()
        {
            // Scene上のModelを取得する
            m_dungeonMapObjects = GameObject.FindGameObjectsWithTag("Cell").ToList();
            m_dungeonMapObjects.ForEach(module =>
            {
                var component = module.GetComponent<Cell>();
                component.Index = new DungeonMapIndex((int)module.transform.localPosition.x, (int)module.transform.localPosition.y, (int)module.transform.localPosition.z);
                //Debug.Log(component.Index + "x" + component.Index.x + "y" + component.Index.y + "z" + component.Index.z);
                m_dungeonMapModules.Add(component);
            });

            // 採掘開始セルを設定し,再帰処理で道を掘る
            var startCell = GetStartCell();
            DiggingInRecursively(startCell);
        }

        /// <summary>
        /// スタート地点を選定する
        /// </summary>
        /// <returns></returns>
        Cell GetStartCell()
        {
            // 指定範囲内の偶数座標をランダムに指定
            var target = new DungeonMapIndex
                (
                Calculator.GetRandomEvenIndex(m_margin, m_width - m_margin),
                Calculator.GetRandomEvenIndex(m_margin, m_height - m_margin),
                Calculator.GetRandomEvenIndex(m_margin, m_depth - m_margin)
                );

            // 指定した座標と同じ座標のmoduleを取得してスタート地点とする
            var startCell = m_dungeonMapModules.Find(item => item.Index == target);
            startCell.Status = CellStatus.Start;
            m_roadEvenCell.Add(startCell);
            StartCell = startCell;

            // playerをスタート地点に立たせる
            var pos = startCell.gameObject.transform.position;
            var player = m_player != null ? m_player : GameObject.FindGameObjectWithTag("Player");
            player.transform.position = new Vector3(pos.x, pos.y, pos.z);

            return startCell;
        }

        /// <summary>
        /// 再帰呼び出しで道を掘る
        /// </summary>
        /// <param name="startCell"></param>
        public void DiggingInRecursively(Cell startCell)
        {

            // 掘る方向
            var dir = GetDirection(startCell);
            // 掘る2マス先のポイントとその中間地点
            DungeonMapIndex target, subTarget;
            subTarget = target = startCell.Index;

            // 観測点からみて掘り進める方向の2cell分取得
            switch (dir)
            {
                case Direction.Right:
                    target.x += targetRange;
                    subTarget.x += subTargetRange;
                    break;
                case Direction.Left:
                    target.x -= targetRange;
                    subTarget.x -= subTargetRange;
                    break;
                case Direction.Top:
                    target.y += targetRange;
                    subTarget.y += subTargetRange;
                    break;
                case Direction.Bottom:
                    target.y -= targetRange;
                    subTarget.y -= subTargetRange;
                    break;
                case Direction.Forward:
                    target.z += targetRange;
                    subTarget.z += subTargetRange;
                    break;
                case Direction.Backward:
                    target.z -= targetRange;
                    subTarget.z -= subTargetRange;
                    break;
                case Direction.None:
                    // もう掘れない場合は自分自身を掘削リストから削除し既存のポイントから掘削を再開する
                    // 最初に壁に当たったCellをゴールとする
                    if (!m_isGoalSet)
                    {
                        m_isGoalSet = true;
                        startCell.Status = CellStatus.Goal;
                        GoalCell = startCell;

                        // ゴール迄の最短通路の最後のCellを保存
                        m_roadToGoal.Add(startCell);

                        //// Goal地点にPortalを設置する
                        //InstantiateWithTemplate(m_goalPortalPrefab, startCell);
                    }

                    // 登録されていない場合行き止まり地点を保存
                    if (m_noPassageCells.Find(cell => cell.Index == startCell.Index) == null)
                    {
                        Debug.Log(startCell.Index + "x" + startCell.Index.x + "y" + startCell.Index.y + "z" + startCell.Index.z);
                        m_noPassageCells.Add(startCell);
                    }
                    // 掘削リストから削除
                    m_roadEvenCell.Remove(startCell);

                    // 掘削リストがまだ残っている時場合再帰呼び出し
                    if (m_roadEvenCell.Count > 0)
                    {
                        DiggingInRecursively(m_roadEvenCell[Calculator.GetRandomEvenIndex(0, m_roadEvenCell.Count)]);
                    }

                    // 掘削ポイントがもうない場合は終了し結果をModelに同期
                    Finish();
                    return;
            }

            // 対象と中間を掘る
            var targetCell = m_dungeonMapModules.Find(cell => cell.Index == target);
            var subTargetCell = m_dungeonMapModules.Find(cell => cell.Index == subTarget);
            targetCell.Status = CellStatus.Road;
            subTargetCell.Status = CellStatus.Road;

            if (!m_isGoalSet)
            {
                // ゴール迄の最短通路を保存
                m_roadToGoal.Add(startCell);
            }
            // 掘削リストに追加
            m_roadEvenCell.Add(targetCell);
            // 掘った先を中心に再帰呼び出し
            DiggingInRecursively(targetCell);
        }

        /// <summary>
        ///仕上げ
        /// </summary>
        private void Finish()
        {
            // mapSizeに合わせてScale変更
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);

            Sync();
            //SpreadCoin();
            SettingPortal();
        }

        void Sync()
        {
            // ダンジョン生成した情報とScene上のオブジェクト群を同期する
            m_dungeonMapModules.ForEach(cell =>
            {
                switch (cell.Status)
                {
                    case CellStatus.Road:
                        RoadCells.Add(cell);
                        cell.gameObject.SetActive(false);
                        break;
                    case CellStatus.Goal:
                    case CellStatus.Start:
                        cell.gameObject.SetActive(false);
                        break;
                    case CellStatus.DigableWall:
                    case CellStatus.UndigableWall:
                        // cellのカラーをランダムに設定
                        cell.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), m_alpha);
                        break;
                }
            });
        }

        /// <summary>
        /// コインをばら撒く
        /// </summary>
        void SpreadCoin()
        {
            var coins = GameObject.FindGameObjectsWithTag("Coin");
            foreach (var coin in coins)
            {
                var cell = m_noPassageCells[Random.Range(0, m_noPassageCells.Count - 1)];
                if (cell.Status == CellStatus.Start)
                {
                    continue;
                }
                coin.transform.SetParent(transform);
                coin.transform.localPosition = cell.transform.localPosition;
                coin.transform.localScale = cell.transform.localScale;
            }
        }

        /// <summary>
        /// Portalを各地に設定する. 設定場所は各行き止まり地点
        /// </summary>
        void SettingPortal()
        {
#if true
            var portals = GameObject.FindGameObjectsWithTag("Portal");
            foreach (var portal in portals)
            {
                var cell = m_noPassageCells[Random.Range(0, m_noPassageCells.Count - 1)];
                if (cell.Status == CellStatus.Start)
                {
                    continue;
                }
                portal.transform.SetParent(transform);
                portal.transform.localPosition = cell.transform.localPosition;
                portal.transform.localScale = cell.transform.localScale;
            }
#else
            var portal = Resources.Load<GameObject>("Portal_green");
            Debug.Log(m_noPassageCells.Count);
            m_noPassageCells.ForEach(cell =>
            {
                var go = Instantiate(portal, transform);
                go.transform.SetParent(transform);
                go.transform.localPosition = cell.transform.localPosition;
                go.transform.localScale = cell.transform.localScale;
            });

#endif

        }

        /// <summary>
        /// Pointを起点に周囲が既に道になっていないか調べる
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        Direction GetDirection(Cell cell)
        {
            var dir = new List<Direction>();
            dir.Add(Direction.Forward);
            dir.Add(Direction.Backward);
            dir.Add(Direction.Right);
            dir.Add(Direction.Left);
            dir.Add(Direction.Top);
            dir.Add(Direction.Bottom);

            while (dir.Count > 0)
            {
                var checkDir = dir[Random.Range(0, dir.Count)];
                var checkIndex = new DungeonMapIndex();

                checkIndex = cell.Index;
                switch (checkDir) // その方向の2つ先のマスをcheckPointに指定
                {
                    case Direction.Right:
                        checkIndex.x += 2;
                        break;
                    case Direction.Left:
                        checkIndex.x -= 2;
                        break;
                    case Direction.Top:
                        checkIndex.y += 2;
                        break;
                    case Direction.Bottom:
                        checkIndex.y -= 2;
                        break;
                    case Direction.Forward:
                        checkIndex.z += 2;
                        break;
                    case Direction.Backward:
                        checkIndex.z -= 2;
                        break;
                    default:
                        break;
                }
                var targetCell = m_dungeonMapModules.Find(item => item.Index == checkIndex);

                if (targetCell.Status == CellStatus.DigableWall) // 対象のマスのステータスが掘削可能ならその向きを返す
                {
                    return checkDir;
                }
                // 対象の方向が掘削不可能だった場合その方向を削除してもう一度チェックする
                dir.Remove(checkDir);
            }
            // 全方向掘削不可能だった場合Noneを返す
            return Direction.None;
        }

        /// <summary>
        /// Instantiate with Template
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="cell"></param>
        GameObject InstantiateWithTemplate(GameObject prefab, Cell cell)
        {
            var go = Instantiate(prefab, transform);
            go.transform.position = cell.transform.localPosition;
            go.transform.localScale = cell.transform.localScale;
            return go;
        }

        /// <summary>
        /// Instantiate with Template
        /// </summary>
        /// <param name="prefab"></param>
        /// <param name="cells"></param>
        List<GameObject> InstantiateWithTemplate(GameObject prefab, List<Cell> cells)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            cells.ForEach(cell =>
            {
                var go = Instantiate(prefab, transform);
                go.transform.position = cell.transform.localPosition;
                go.transform.localScale = cell.transform.localScale;
                gameObjects.Add(go);
            });
            return gameObjects;
        }
    }

    /// <summary>
    /// そのmoduleのindex
    /// </summary>
    public struct DungeonMapIndex
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public DungeonMapIndex(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合true
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(DungeonMapIndex a, DungeonMapIndex b)
        {

            // x,y,z全ての要素が一致していた場合true
            if (a.x == b.x & a.y == b.y && a.z == b.z)
            {
                return true;
            }
            // でなければfalse
            return false;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(DungeonMapIndex a, DungeonMapIndex b)
        {
            return !(a == b);
        }
    }

    /// <summary>
    /// 掘削方向
    /// </summary>
    public enum Direction
    {
        Right,
        Left,
        Top,
        Bottom,
        Forward,
        Backward,
        None,
    }

    /// <summary>
    /// そのCellのステータス
    /// </summary>

    public enum CellStatus
    {
        Road,
        DigableWall,
        UndigableWall,
        Goal,
        Start,
    }
}
#else
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReviewGames
{

    /// <summary>
    /// 3D迷路の自動生成
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        /// <summary>DungeonMapのオブジェクト群</summary>
        public List<GameObject> DungeonMapObjects { get; private set; }
        /// <summary>DungeonMapのコンポーネント群</summary>
        public List<Cell> DungeonMapComponents { get; private set; }
        /// <summary>道の偶数地点</summary>
        public List<Cell> RoadEvenCell { get; private set; }
        /// <summary>スタート地点</summary>
        public Cell StartPoint { get; private set; }
        /// <summary>ゴール地点</summary>
        public Cell GoalPoint { get; private set; }

        StateManager m_stateManager;

        /// <summary>map size</summary>
        [Header("Can be changed during execution.")]
        [SerializeField] float m_mapSize;

        /// <summary>幅</summary>
        [Header("Can't be changed during execution.")]
        [SerializeField] int m_width;
        /// <summary>高さ</summary>
        [SerializeField] int m_height;
        /// <summary>奥行</summary>
        [SerializeField] int m_depth;
        /// <summary>コライダーを必要とするかどうか</summary>
        [SerializeField] bool m_needToCollider;
        /// <summary>迷路の外周分の余白</summary>
        [SerializeField] int margin = 2;
        /// <summary>ゴール地点を設定しているかどうか</summary>
        bool m_isGoalSet;
        /// <summary>ターゲットにする範囲</summary>
        const int targetRange = 2;
        /// <summary>サブターゲットにする範囲</summary>
        const int subTargetRange = 1;

        private void Awake()
        {
            //外周分を加算
            m_width += margin;
            m_height += margin;
            m_depth += margin;

            RoadEvenCell = new List<Cell>();
            DungeonMapObjects = new List<GameObject>();
            DungeonMapComponents = new List<Cell>();
            m_stateManager = StateManager.Instance;

            Setup();

        }

        private void OnValidate()
        {
            // Play中以外スルー
            if (!Application.isPlaying)
            {
                return;
            }
            // mapSizeに合わせてScale変更
            transform.localScale = new Vector3(m_mapSize, m_mapSize, m_mapSize);
        }



        /// <summary>
        /// マップ配列の各要素に役割を割り当てる
        /// </summary>
        public void Setup()
        {
            // 各面において両端の場合は掘削不可能な壁,それ以外は掘削可能な壁とする
            for (int y = 0; y < m_height; y++)
            {
                for (int z = 0; z < m_depth; z++)
                {
                    for (int x = 0; x < m_width; x++)
                    {
                        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                        cube.transform.SetParent(transform);
                        cube.transform.localScale = Vector3.one;
                        cube.transform.localPosition = new Vector3(x, y, z);
                        // もしコライダーが必要なければ消す
                        if (!m_needToCollider)
                        {
                            Destroy(cube.GetComponent<BoxCollider>());
                        }
                        // Componentを追加,Listに追加,各要素初期化
                        var component = cube.AddComponent<Cell>();
                        DungeonMapObjects.Add(cube);
                        DungeonMapComponents.Add(component);
                        // 各面において余白の場合掘削不可能壁に. それ以外は掘削可能壁に設定.
                        if (y < margin | y >= m_height - margin
                            | z < margin | z >= m_depth - margin
                            | x < margin | x >= m_width - margin)
                        {
                            component.Status = CellStatus.UndigableWall;
                        }
                        else
                        {
                            component.Status = CellStatus.DigableWall;
                        }
                        component.Index = new DungeonMapIndex(x, y, z);
                    }
                }
            }
            // 最初のポイントを作成し,再帰処理で道の作成を開始
            DiggingInRecursively(GetFirstPoint());
        }

        /// <summary>
        /// 迷路の開始地点を決める. 開始地点は外周以外の偶数を選択
        /// </summary>
        Cell GetFirstPoint()
        {
            var target = new DungeonMapIndex
                (
                GetRandomEvenIndex(margin, m_width - margin),
                GetRandomEvenIndex(margin, m_height - margin),
                GetRandomEvenIndex(margin, m_depth - margin)
                );

            var startCell = DungeonMapComponents.Find(item => item.Index == target);
            startCell.Status = CellStatus.Start;
            RoadEvenCell.Add(startCell);
            StartPoint = startCell;
            return startCell;
        }

        /// <summary>
        /// 再帰呼び出しで道を掘る
        /// </summary>
        /// <param name="startPoint"></param>
        public void DiggingInRecursively(Cell startPoint)
        {

            // 掘る方向
            var dir = GetDirection(startPoint);
            // 掘る2マス先のポイントとその中間地点
            DungeonMapIndex target, subTarget;
            subTarget = target = startPoint.Index;
            Debug.Log("startCell is : " + target.x + "," + target.y + "," + target.z);

            switch (dir)
            {
                case Direction.Right:
                    target.x += targetRange;
                    subTarget.x += subTargetRange;
                    break;
                case Direction.Left:
                    target.x -= targetRange;
                    subTarget.x -= subTargetRange;
                    break;
                case Direction.Top:
                    target.y += targetRange;
                    subTarget.y += subTargetRange;
                    break;
                case Direction.Bottom:
                    target.y -= targetRange;
                    subTarget.y -= subTargetRange;
                    break;
                case Direction.Forward:
                    target.z += targetRange;
                    subTarget.z += subTargetRange;
                    break;
                case Direction.Backward:
                    target.z -= targetRange;
                    subTarget.z -= subTargetRange;
                    break;
                case Direction.None:
                    // もう掘れない場合は自分自身を掘削リストから削除し、既存のポイントから掘削を再開する
                    // 最初に壁に当たった一回目をゴールとする
                    if (!m_isGoalSet)
                    {
                        m_isGoalSet = true;
                        startPoint.Status = CellStatus.Goal;
                        GoalPoint = startPoint;
                    }
                    RoadEvenCell.Remove(startPoint);

                    if (RoadEvenCell.Count > 0)
                    {
                        // 再帰呼び出し
                        DiggingInRecursively(RoadEvenCell[GetRandomEvenIndex(0, RoadEvenCell.Count)]);
                    }
                    // 掘削ポイントがもうない場合は終了し結果をオブジェクトに同期
                    Sync();

                    return;
            }
            // 対象と中間を掘る
            var targetCell = DungeonMapComponents.Find(item => item.Index == target);
            var subTargetCell = DungeonMapComponents.Find(item => item.Index == subTarget);
            targetCell.Status = CellStatus.Road;
            subTargetCell.Status = CellStatus.Road;

            // 掘削リストに追加
            RoadEvenCell.Add(targetCell);
            // 掘った先を中心に再帰呼び出し
            DiggingInRecursively(targetCell);
        }

        private void Sync()
        {
            DungeonMapComponents.ForEach(item =>
            {
                switch (item.Status)
                {
                    case CellStatus.Road:
                    case CellStatus.Goal:
                    case CellStatus.Start:
                        item.gameObject.SetActive(false);
                        break;
                    case CellStatus.DigableWall:
                    case CellStatus.UndigableWall:
                        item.GetComponent<MeshRenderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 0.1f);
                        break;
                }
            });
        }



        /// <summary>
        /// 範囲内でランダムな偶数値を取得
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        int GetRandomEvenIndex(int min, int max)
        {
            int evenNum = 1;
            while (evenNum % 2 == 1)
            {
                evenNum = Random.Range(min, max);
            }
            return evenNum;
        }

        /// <summary>
        /// Pointを起点に周囲が既に道になっていないか調べる
        /// </summary>
        /// <param name="dmo"></param>
        /// <returns></returns>
        Direction GetDirection(Cell dmo)
        {
            var dir = new List<Direction>();
            dir.Add(Direction.Forward);
            dir.Add(Direction.Backward);
            dir.Add(Direction.Right);
            dir.Add(Direction.Left);
            dir.Add(Direction.Top);
            dir.Add(Direction.Bottom);

            while (dir.Count > 0)
            {
                var checkDir = dir[Random.Range(0, dir.Count)];
                var checkIndex = new DungeonMapIndex();

                checkIndex = dmo.Index;
                switch (checkDir) // その方向の2つ先のマスをcheckPointに指定
                {
                    case Direction.Right:
                        checkIndex.x += 2;
                        break;
                    case Direction.Left:
                        checkIndex.x -= 2;
                        break;
                    case Direction.Top:
                        checkIndex.y += 2;
                        break;
                    case Direction.Bottom:
                        checkIndex.y -= 2;
                        break;
                    case Direction.Forward:
                        checkIndex.z += 2;
                        break;
                    case Direction.Backward:
                        checkIndex.z -= 2;
                        break;
                    default:
                        break;
                }
                Debug.Log("checkIndex is : " + checkIndex.x + "," + checkIndex.y + "," + checkIndex.z);
                var targetCell = DungeonMapComponents.Find(item => item.Index == checkIndex);

                if (targetCell.Status == CellStatus.DigableWall) // 対象のマスのステータスが掘削可能ならその向きを返す
                {
                    return checkDir;
                }
                // 対象の方向が掘削不可能だった場合その方向を削除してもう一度チェックする
                dir.Remove(checkDir);
            }
            // 全方向掘削不可能だった場合Noneを返す
            return Direction.None;
        }
    }

    public struct DungeonMapIndex
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }

        public DungeonMapIndex(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合true
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(DungeonMapIndex a, DungeonMapIndex b)
        {

            // x,y,z全ての要素が一致していた場合true
            if (a.x == b.x & a.y == b.y && a.z == b.z)
            {
                return true;
            }
            // でなければfalse
            return false;
        }

        /// <summary>
        /// x,y,z全ての要素が一致していた場合,若しくは同じインスタンスの参照同士の場合false
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(DungeonMapIndex a, DungeonMapIndex b)
        {
            return !(a == b);
        }
    }

    // 掘削方向
    public enum Direction
    {
        Right,
        Left,
        Top,
        Bottom,
        Forward,
        Backward,
        None,
    }

    /// <summary>
    /// そのマスのステータス
    /// </summary>

    public enum CellStatus
    {
        Road,
        DigableWall,
        UndigableWall,
        Goal,
        Start,
    }
}
#endif
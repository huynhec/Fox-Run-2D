using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    static class GameObjectCreation
    {
        private static class Styles
        {
            public static readonly string rectangularCreateUndo = L10n.Tr("Create Tilemap");
            public static readonly string pointTopHexagonCreateUndo = L10n.Tr("Hexagonal Point Top Tilemap");
            public static readonly string flatTopHexagonCreateUndo = L10n.Tr("Hexagonal Flat Top Tilemap");
            public static readonly string isometricCreateUndo = L10n.Tr("Isometric Tilemap");
            public static readonly string isometricZAsYCreateUndo = L10n.Tr("Isometric Z As Y Tilemap");
            public static readonly string copyFromTilePaletteUndo = L10n.Tr("Tilemap with Tile Palette Settings");
        }

        const int k_MenuPriority = 3;

        internal static string[] CreateTilemapTargetsNames = new[]
        {
            "From Tile Palette"
            , "Rectangular Tilemap"
            , "Hexagonal Point Top Tilemap"
            , "Hexagonal Flat Top Tilemap"
            , "Isometric Tilemap"
            , "Isometric Z As Y Tilemap"
        };

        internal static void CreateTilemapTargets(int index)
        {
            switch (index)
            {
                case 1:
                    CreateRectangularTilemap();
                    break;
                case 2:
                    CreateHexagonalPointTopTilemap();
                    break;
                case 3:
                    CreateHexagonalFlatTopTilemap();
                    break;
                case 4:
                    CreateIsometricTilemap();
                    break;
                case 5:
                    CreateIsometricZAsYTilemap();
                    break;
                case 0:
                    {
                        var palette = GridPaintingState.palette;
                        if (palette == null)
                            return;

                        var grid = palette.GetComponentInChildren<Grid>();
                        if (grid == null)
                            return;

                        GameObject newGameObject = null;
                        switch (grid.cellLayout)
                        {
                            case GridLayout.CellLayout.Rectangle:
                                newGameObject = CreateRectangularTilemapInternal(Styles.copyFromTilePaletteUndo);
                                break;
                            case GridLayout.CellLayout.Hexagon:
                                newGameObject = CreateHexagonalTilemapInternal(grid.cellSwizzle,
                                    Styles.copyFromTilePaletteUndo, grid.cellSize);
                                break;
                            case GridLayout.CellLayout.Isometric:
                                newGameObject = CreateIsometricTilemapInternal(GridLayout.CellLayout.Isometric, Styles.copyFromTilePaletteUndo);
                                break;
                            case GridLayout.CellLayout.IsometricZAsY:
                                newGameObject = CreateIsometricTilemapInternal(GridLayout.CellLayout.IsometricZAsY, Styles.copyFromTilePaletteUndo);
                                break;
                        }
                        if (newGameObject == null)
                            return;

                        var newGrid = newGameObject.GetComponentInChildren<Grid>();
                        if (newGrid == null)
                            return;

                        var tilemap = palette.GetComponentInChildren<Tilemap>();
                        var newTilemap = newGameObject.GetComponentInChildren<Tilemap>();
                        var tilemapRenderer = palette.GetComponentInChildren<TilemapRenderer>();
                        var newTilemapRenderer = newGameObject.GetComponentInChildren<TilemapRenderer>();

                        Undo.RecordObjects(new Object[] {newGrid, newTilemap, newTilemapRenderer}, Styles.copyFromTilePaletteUndo);

                        var newSize = grid.cellSize;
                        var paletteAsset = GridPaletteUtility.GetGridPaletteFromPaletteAsset(palette);
                        if (paletteAsset != null && paletteAsset.cellSizing == GridPalette.CellSizing.Automatic)
                        {
                            newSize = GridPaletteUtility.CalculateAutoCellSize(grid, grid.cellSize);
                        }

                        if (tilemap != null && newTilemap != null)
                        {
                            newTilemap.animationFrameRate = tilemap.animationFrameRate;
                            newTilemap.color = tilemap.color;
                            newTilemap.tileAnchor = tilemap.tileAnchor;
                            newTilemap.orientation = tilemap.orientation;
                            newTilemap.orientationMatrix = tilemap.orientationMatrix;
                        }

                        if (tilemapRenderer != null && newTilemapRenderer != null)
                        {
                            newTilemapRenderer.mode = tilemapRenderer.mode;
                            newTilemapRenderer.chunkSize = tilemapRenderer.chunkSize;
                            newTilemapRenderer.chunkCullingBounds = tilemapRenderer.chunkCullingBounds;
                            newTilemapRenderer.detectChunkCullingBounds = tilemapRenderer.detectChunkCullingBounds;
                            newTilemapRenderer.sortOrder = tilemapRenderer.sortOrder;
                            newTilemapRenderer.maskInteraction = tilemapRenderer.maskInteraction;
                            newTilemapRenderer.sortingOrder = tilemapRenderer.sortingOrder;
                            newTilemapRenderer.sortingLayerID = tilemapRenderer.sortingLayerID;
                        }

                        newGrid.cellSwizzle = grid.cellSwizzle;
                        if (newGrid.cellLayout != GridLayout.CellLayout.Hexagon)
                            newGrid.cellGap = grid.cellGap;
                        newGrid.cellSize = newSize;
                    }
                    break;
            }
        }

        [MenuItem("GameObject/2D Object/Tilemap/Rectangular", priority = k_MenuPriority)]
        internal static void CreateRectangularTilemap()
        {
            CreateRectangularTilemapInternal(Styles.rectangularCreateUndo);
        }

        [MenuItem("GameObject/2D Object/Tilemap/Hexagonal Point Top", priority = k_MenuPriority)]
        internal static void CreateHexagonalPointTopTilemap()
        {
            CreateHexagonalTilemapInternal(GridLayout.CellSwizzle.XYZ, Styles.pointTopHexagonCreateUndo, new Vector3(0.8659766f, 1, 1));
        }

        [MenuItem("GameObject/2D Object/Tilemap/Hexagonal Flat Top", priority = k_MenuPriority)]
        internal static void CreateHexagonalFlatTopTilemap()
        {
            CreateHexagonalTilemapInternal(GridLayout.CellSwizzle.YXZ, Styles.flatTopHexagonCreateUndo, new Vector3(0.8659766f, 1, 1));
        }

        [MenuItem("GameObject/2D Object/Tilemap/Isometric", priority = k_MenuPriority)]
        internal static void CreateIsometricTilemap()
        {
            CreateIsometricTilemapInternal(GridLayout.CellLayout.Isometric, Styles.isometricCreateUndo);
        }

        [MenuItem("GameObject/2D Object/Tilemap/Isometric Z as Y", priority = k_MenuPriority)]
        internal static void CreateIsometricZAsYTilemap()
        {
            CreateIsometricTilemapInternal(GridLayout.CellLayout.IsometricZAsY, Styles.isometricZAsYCreateUndo);
        }

        private static GameObject CreateIsometricTilemapInternal(GridLayout.CellLayout isometricLayout, string undoMessage)
        {
            var root = FindOrCreateRootGrid();
            var uniqueName = GameObjectUtility.GetUniqueNameForSibling(root.transform, "Tilemap");
            var tilemapGO = ObjectFactory.CreateGameObject(uniqueName, typeof(Tilemap), typeof(TilemapRenderer));
            tilemapGO.transform.SetParent(root.transform);
            tilemapGO.transform.position = Vector3.zero;

            var grid = root.GetComponent<Grid>();
            // Case 1071703: Do not reset cell size if adding a new Tilemap to an existing Grid of the same layout
            if (isometricLayout != grid.cellLayout)
            {
                Undo.RecordObject(grid, undoMessage);
                grid.cellLayout = isometricLayout;
                grid.cellSize = new Vector3(1.0f, 0.5f, 1.0f);
            }

            var tilemapRenderer = tilemapGO.GetComponent<TilemapRenderer>();
            tilemapRenderer.sortOrder = TilemapRenderer.SortOrder.TopRight;

            GridPaintingState.FlushCache();
            Selection.activeObject = tilemapGO;
            Undo.RegisterCreatedObjectUndo(tilemapGO, undoMessage);
            return root;
        }

        private static GameObject CreateHexagonalTilemapInternal(GridLayout.CellSwizzle swizzle, string undoMessage, Vector3 cellSize)
        {
            var root = FindOrCreateRootGrid();
            var uniqueName = GameObjectUtility.GetUniqueNameForSibling(root.transform, "Tilemap");
            var tilemapGO = ObjectFactory.CreateGameObject(uniqueName, typeof(Tilemap), typeof(TilemapRenderer));
            tilemapGO.transform.SetParent(root.transform);
            tilemapGO.transform.position = Vector3.zero;

            var grid = root.GetComponent<Grid>();
            Undo.RecordObject(grid, undoMessage);
            grid.cellLayout = Grid.CellLayout.Hexagon;
            grid.cellSwizzle = swizzle;
            grid.cellSize = cellSize;
            var tilemap = tilemapGO.GetComponent<Tilemap>();
            tilemap.tileAnchor = Vector3.zero;

            GridPaintingState.FlushCache();
            Selection.activeObject = tilemapGO;
            Undo.RegisterCreatedObjectUndo(tilemapGO, undoMessage);
            return root;
        }

        private static GameObject CreateRectangularTilemapInternal(string undoMessage)
        {
            var root = FindOrCreateRootGrid();
            var uniqueName = GameObjectUtility.GetUniqueNameForSibling(root.transform, "Tilemap");
            var tilemapGO = ObjectFactory.CreateGameObject(uniqueName, typeof(Tilemap), typeof(TilemapRenderer));
            Undo.SetTransformParent(tilemapGO.transform, root.transform, "");
            tilemapGO.transform.position = Vector3.zero;

            var grid = root.GetComponent<Grid>();
            if (Grid.CellLayout.Rectangle != grid.cellLayout)
            {
                Undo.RecordObject(grid, undoMessage);
                grid.cellLayout = Grid.CellLayout.Rectangle;
            }

            GridPaintingState.FlushCache();
            Selection.activeObject = tilemapGO;
            Undo.SetCurrentGroupName(undoMessage);
            return root;
        }

        private static GameObject FindOrCreateRootGrid()
        {
            GameObject gridGO = null;

            // Check if active object has a Grid and can be a parent for the Tile Map
            if (Selection.activeObject is GameObject)
            {
                var go = (GameObject)Selection.activeObject;
                var parentGrid = go.GetComponentInParent<Grid>();
                if (parentGrid != null)
                {
                    gridGO = parentGrid.gameObject;
                }
            }

            // If neither the active object nor its parent has a grid, create a grid for the tilemap
            if (gridGO == null)
            {
                gridGO = ObjectFactory.CreateGameObject("Grid", typeof(Grid));
                gridGO.transform.position = Vector3.zero;

                var grid = gridGO.GetComponent<Grid>();
                grid.cellSize = new Vector3(1.0f, 1.0f, 0.0f);
                Undo.SetCurrentGroupName("Create Grid");
            }

            return gridGO;
        }
    }
}

package mg.gui;

import java.io.IOException;
import javafx.geometry.Point2D;
import javafx.scene.Cursor;
import javafx.scene.Group;
import javafx.scene.Node;
import javafx.scene.control.Button;
import javafx.scene.control.CheckBox;
import javafx.scene.control.ComboBox;
import javafx.scene.control.Control;
import javafx.scene.control.Label;
import javafx.scene.control.ListView;
import javafx.scene.control.ScrollPane;
import javafx.scene.control.SplitPane;
import javafx.scene.control.TextField;
import javafx.scene.control.Tooltip;
import javafx.scene.layout.BorderPane;
import javafx.scene.layout.FlowPane;
import javafx.scene.layout.HBox;
import javafx.scene.layout.Pane;
import javafx.scene.layout.StackPane;
import javafx.scene.layout.VBox;
import javafx.scene.shape.Line;
import javafx.scene.transform.Scale;
import mg.EnumContainer.LOG_TYPE;
import mg.PropertyType;
import static mg.PropertyType.UPDATE_ERROR_MESSAGE;
import static mg.PropertyType.UPDATE_ERROR_TITLE;
import mg.data.DataManager;
import mg.data.DataModifyLog;
import mg.data.obj.DataHandler;
import mg.data.obj.DesignObj;
import mg.data.obj.Point;
import mg.data.obj.PolygonObj;
import mg.file.FileManager;
import properties_manager.PropertiesManager;
import saf.AppTemplate;
import saf.components.AppWorkspaceComponent;
import saf.ui.AppGUI;
import saf.ui.AppMessageDialogSingleton;

/**
 * This class serves as the workspace component for this application, providing
 * the user interface controls for editing work.
 *
 * @author Richard McKenna
 * @author Yun Joon Soh
 * @version 1.0
 */
public class Workspace extends AppWorkspaceComponent {

    // final static constants
    final static double VGAP = 50;
    final static double MAX_RIGHT_CONTROL_PANEL = 500;

    public static void setCursor(Cursor TYPE) {
        gui.getPrimaryScene().setCursor(TYPE);
    }

    // HERE'S THE APP
    private static AppTemplate app;

    // IT KNOWS THE GUI IT IS PLACED INSIDE
    private static AppGUI gui;

    private static BorderPane workspace;

    Button toPhotoButton, toCodeButton;
    Button addClassButton, addInterfaceButton, buildPolygon, redoButton, undoButton;
    Button saveAsButton;

    HBox editToolbarPane, viewToolbarPane, fileToolbarPane;

    Button zoomInButton, zoomOutButton;
    CheckBox grid, snap;

    ScrollPane viewAreaScrollPane;
    StackPane viewAreaStackPane;
    Pane gridPane;
    static Pane viewAreaPane;

    VBox rightControlPanel, variablePane, methodsPane;
    VBox classNamePane, packageNamePane, parentSelectionPane;
    HBox upperHalf;
    VBox inputPane;
    HBox parentSelectionControlPane;

    ScrollPane parentTableScrollPane;
    TextField classNameTF, packageNameTF;
    ComboBox<String> parentListComboBox;
    Button addToParentList, removeFromParentList;
    ListView<String> parentListView;

    Button addVariable, removeVariable, addMethods, removeMethods, addArgument, removeArgument;

    ScrollPane varTableScrollPane, metTableScrollPane, argTableScrollPane;
    SplitPane splitMetArg, splitPSelectPList;

    static Point2D startDrag, endDrag;
    Label SceneX, SceneY, GetX, GetY;
    Label SceneXVal, SceneYVal, GetXVal, GetYVal;

    FlowPane bottomStatusPane;

    /**
     * Constructor for initializing the workspace, note that this constructor
     * will fully setup the workspace user interface for use.
     *
     * @param initApp The application this workspace is part of.
     *
     * @throws IOException Thrown should there be an error loading application
     * data for setting up the user interface.
     */
    public Workspace(AppTemplate initApp) throws IOException {
        // KEEP THIS FOR LATER
        app = initApp;

        // KEEP THE GUI FOR LATER
        gui = app.getGUI();
        workspace = gui.getAppPane();

        setTop();

        setCenter();

        setRight();

        setBottom();

        ((BorderPane) workspace).setBottom(bottomStatusPane);

    }

    private void setTop() {
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        fileToolbarPane = gui.getFileToolbarPane();

        /*SAVE AS*/
        saveAsButton = AppGUI.initChildButton(fileToolbarPane, PropertyType.SAVE_AS_ICON.toString(), PropertyType.SAVE_AS_TOOLTIP.toString(), true, 3);
        saveAsButton.setOnAction(e -> {
            WorkspaceHandler.handleSaveAsRequest((FileManager) app.getFileComponent(), (DataManager) app.getDataComponent(), gui);
        });
        saveAsButton.disableProperty().bind(workspaceDeactivated);

        /*TO CODE*/
        toCodeButton = AppGUI.initChildButton(fileToolbarPane, PropertyType.EXPORT_TO_PHOTO_ICON.toString(), PropertyType.EXPORT_TO_PHOTO_TOOLTIP.toString(), true, 4);
        toCodeButton.setOnAction(e -> {
            DataHandler.handleToCode();
        });
        toCodeButton.disableProperty().bind(workspaceDeactivated);

        /*TO PHOTO*/
        toPhotoButton = AppGUI.initChildButton(fileToolbarPane, PropertyType.EXPORT_TO_CODE_ICON.toString(), PropertyType.EXPORT_TO_CODE_TOOLTIP.toString(), true, 5);
        toPhotoButton.setOnAction(e -> {
            WorkspaceHandler.handleSnapshotRequest(viewAreaStackPane, gui);
        });
        toPhotoButton.disableProperty().bind(workspaceDeactivated);

        /*EDIT TOOLBAR*/
        editToolbarPane = new HBox();

        buildPolygon = gui.initChildButton(editToolbarPane, PropertyType.REMOVE_ICON.toString(), PropertyType.REMOVE_TOOLTIP.toString(), false);
        buildPolygon.setOnAction(e -> {
            WorkspaceHandler.handleGeneratePolygon();
            reloadWorkspace();
//            DataManager.pushToUndoLog(new DataModifyLog(DataManager.getSelectedDObj().get(), LOG_TYPE.DObj_ADD));
        });
//        buildPolygon.disableProperty().bind(workspaceDeactivated.or(DataManager.IS_DBOX_SELECTED.not()));

        undoButton = gui.initChildButton(editToolbarPane, PropertyType.UNDO_ICON.toString(), PropertyType.UNDO_TOOLTIP.toString(), true);
        undoButton.setOnAction(e -> {
            DataHandler.handleUndo();
            
        });

        redoButton = gui.initChildButton(editToolbarPane, PropertyType.REDO_ICON.toString(), PropertyType.REDO_TOOLTIP.toString(), true);
        redoButton.setOnAction(e -> {
            DataHandler.handleRedo();
        });

        /*VIEW TOOLBAR*/
        viewToolbarPane = new HBox();

        zoomInButton = gui.initChildButton(viewToolbarPane, PropertyType.ZOOMIN_ICON.toString(), PropertyType.ZOOMIN_TOOLTIP.toString(), true);
        zoomInButton.setOnAction(e -> {
            WorkspaceHandler.zoomin();
        });
        zoomInButton.disableProperty().bind(workspaceDeactivated);

        zoomOutButton = gui.initChildButton(viewToolbarPane, PropertyType.ZOOMOUT_ICON.toString(), PropertyType.ZOOMOUT_TOOLTIP.toString(), true);
        zoomOutButton.setOnAction(e -> {
            WorkspaceHandler.zoomout();
        });
        zoomOutButton.disableProperty().bind(workspaceDeactivated);

        grid = new CheckBox(props.getProperty(PropertyType.GRID_LABEL.toString()));
        grid.setTooltip(new Tooltip(PropertyType.GRID_TOOLTIP.toString()));
        (WorkspaceHandler.GRID_STATUS).bind(grid.selectedProperty());
        grid.disableProperty().bind(workspaceDeactivated);

        snap = new CheckBox(props.getProperty(PropertyType.SNAP_LABEL.toString()));
        snap.setTooltip(new Tooltip(PropertyType.SNAP_TOOLTIP.toString()));
        (WorkspaceHandler.SNAP_STATUS).bind(snap.selectedProperty());
        snap.disableProperty().bind(workspaceDeactivated);

        VBox temp = new VBox(4, grid, snap);
        viewToolbarPane.getChildren().add(temp);

        // add to top border
        gui.getToolbarPane().getChildren().addAll(editToolbarPane, viewToolbarPane);
    }

    public static final String DEFAULT_ARGUMENT_NAME = "ARG1";
    public static final String DEFAULT_ARG_TYPE = "int";

    private void setRight() {
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        /**
         * **************************************************************************
         * RIGHT CONTROL PANE
         */
        classNameTF = new TextField();
        classNameTF.disableProperty().bind(workspaceDeactivated.or(DataManager.IS_DBOX_SELECTED.not()));

        packageNameTF = new TextField();
        packageNameTF.disableProperty().bind(workspaceDeactivated.or(DataManager.IS_DBOX_SELECTED.not()));

        parentListComboBox = new ComboBox<>();
        parentListComboBox.setEditable(true);

        parentListComboBox.setMinWidth(200);
        parentListComboBox.disableProperty().bind(workspaceDeactivated.or(DataManager.IS_DBOX_SELECTED.not()));

        parentListView = new ListView<>();
        parentListView.disableProperty().bind(workspaceDeactivated.or(DataManager.IS_DBOX_SELECTED.not()));

        parentTableScrollPane = new ScrollPane(parentListView);
        parentTableScrollPane.setFitToHeight(true);
        parentTableScrollPane.setFitToWidth(true);

        classNamePane = setLabelControlPair(PropertyType.CLASS_NAME_LABEL, classNameTF);
        packageNamePane = setLabelControlPair(PropertyType.PACKAGE_NAME_LABEL, packageNameTF);

        String labelStr = props.getProperty(PropertyType.PARENT_SELECTION_LABEL);
        Label parentListLabel = new Label(labelStr);
        parentListLabel.setMinWidth(200);
        parentListLabel.getStyleClass().add(CLASS_HEADING_LABEL);

        parentSelectionControlPane = new HBox(5);
        parentSelectionControlPane.getChildren().add(parentListComboBox);

        addToParentList = gui.initChildButton(parentSelectionControlPane, PropertyType.ADD_PARENT_ICON.toString(), PropertyType.ADD_PARENT_TOOLTIP.toString(), true);

        addToParentList.disableProperty().bind(workspaceDeactivated.or(parentListComboBox.getSelectionModel().selectedItemProperty().isNull()).or(DataManager.IS_DBOX_SELECTED.not()));

        removeFromParentList = gui.initChildButton(parentSelectionControlPane, PropertyType.REMOVE_PARENT_ICON.toString(), PropertyType.ADD_PARENT_TOOLTIP.toString(), true);

        removeFromParentList.disableProperty().bind(workspaceDeactivated.or(parentListView.getSelectionModel().selectedItemProperty().isNull()));

        parentSelectionPane = new VBox(5, parentListLabel, parentSelectionControlPane);

        inputPane = new VBox(classNamePane, packageNamePane, parentSelectionPane);
        upperHalf = new HBox(10, inputPane, parentTableScrollPane);

        /**
         * ---------------------------------------------------------------------
         * //
         */
//        rightControlPanel = new VBox(10, upperHalf, variablePane, methodsPane); 
//        rightControlPanel.setStyle("-fx-background-color: gray;");
//        rightControlPanel.setMaxWidth(MAX_RIGHT_CONTROL_PANEL);
        ((BorderPane) workspace).setRight(rightControlPanel);
    }

    private void setCenter() {
        viewAreaPane = new Pane();
        viewAreaPane.setOnMouseMoved(e -> {
            SceneXVal.setText(e.getSceneX() + "");
            SceneYVal.setText(e.getSceneY() + "");
            GetXVal.setText(e.getX() + "");
            GetYVal.setText(e.getY() + "");
        });

        viewAreaPane.setMinWidth(2000);
        viewAreaPane.setMinHeight(2000);

        Group gridGroup = getGrid(2000, 2000);
        gridGroup.visibleProperty().bind(grid.selectedProperty());
        gridPane = new Pane(gridGroup);
        Group zoomGroup = new Group(gridPane, viewAreaPane);
        Group tempGroup = new Group(zoomGroup);
        viewAreaStackPane = new StackPane(tempGroup);
        WorkspaceHandler.SCALE_X.set(zoomGroup.getScaleX());
        WorkspaceHandler.SCALE_Y.set(zoomGroup.getScaleY());

        Scale scaleTransform = new Scale(zoomGroup.getScaleX(), zoomGroup.getScaleY(), 0, 0);

        zoomGroup.getTransforms().add(scaleTransform);
        zoomGroup.scaleXProperty().bind(WorkspaceHandler.SCALE_X);
        zoomGroup.scaleYProperty().bind(WorkspaceHandler.SCALE_Y);

        viewAreaScrollPane = new ScrollPane(viewAreaStackPane);

//        viewAreaScrollPane.setOnKeyPressed(e -> {
//            System.out.println("KeyPressed: " + e.toString());
//            switch (e.getText()) {
//                case "M":
//                case "m":
//                    System.out.println("M, m pressed");
//                    if (!(DataManager.getSelectedDObj().get() instanceof Point)) {
//                        break;
//                    }
//                    Point selectedPoint = (Point) DataManager.getSelectedDObj().get();
//                    if (selectedPoint == null) {
//                        System.out.println("Selected Point is Null");
//                        return;
//                    }
//
//                    LineSeg ls = selectedPoint.getParentLineSeg();
//
//                    break;
//                case "S":
//                case "s":
//                    System.out.println("S, s pressed");
//                    if (!(DataManager.getSelectedDObj().get() instanceof LineSeg)) {
//                        break;
//                    }
//
//                    LineSeg selectedLineSeg = (LineSeg) DataManager.getSelectedDObj().get();
//
//                    break;
//            }
//            reloadWorkspace();
//        });
        viewAreaScrollPane.setOnMouseClicked(e -> {
            WorkspaceHandler.handleMouseClick(e);
            reloadWorkspace();
        });

        ((BorderPane) workspace).setCenter(viewAreaScrollPane);
    }

    private void setBottom() {
        /**
         * **************************************************************************
         * Bottom Status PANE
         */
        bottomStatusPane = new FlowPane();

        SceneX = new Label(" getSceneX: ");
        SceneY = new Label(" getSceneY: ");
        GetX = new Label(" getX: ");
        GetY = new Label(" getY: ");
        SceneXVal = new Label();
        SceneYVal = new Label();
        GetXVal = new Label();
        GetYVal = new Label();

        bottomStatusPane.getChildren().addAll(SceneX, SceneXVal, SceneY, SceneYVal, GetX, GetXVal, GetY, GetYVal);
    }

    private VBox setLabelControlPair(PropertyType propertyName, Control bot) {
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        String labelStr = props.getProperty(propertyName);

        Label l = new Label(labelStr);
        l.setMinWidth(200);
        l.getStyleClass().add(CLASS_HEADING_LABEL);

        bot.setMinWidth(200);
        VBox toRet = new VBox(l, bot);

        return toRet;
    }

    /**
     * This function specifies the CSS style classes for all the UI components
     * known at the time the workspace is initially constructed. Note that the
     * tag editor controls are added and removed dynamicaly as the application
     * runs so they will have their style setup separately.
     */
    @Override
    public void initStyle() {
        // NOTE THAT EACH CLASS SHOULD CORRESPOND TO
        // A STYLE CLASS SPECIFIED IN THIS APPLICATION'S
        // CSS FILE
        editToolbarPane.getStyleClass().add(CLASS_TOOLBAR_PANE);
        viewToolbarPane.getStyleClass().add(CLASS_TOOLBAR_PANE);
        viewAreaScrollPane.setStyle("-fx-background-color: gray;");
        fileToolbarPane.setStyle("-fx-background-color: blue;");
        editToolbarPane.setStyle("-fx-background-color: green;");
        viewToolbarPane.setStyle("-fx-background-color: red;");
    }

    @Override
    public void reloadWorkspace() {
        try {
            //clear out old stuff

            //set cursor
            gui.getPrimaryScene().setCursor(Cursor.DEFAULT);

            //load value
            reloadCanvas();

            updateCustomToolbarControl(workspaceDeactivated.get());
            initStyle();

        } catch (Exception e) {
            System.out.println("ReloadWorkspace failed" + e.getMessage());
            e.printStackTrace();
            AppMessageDialogSingleton dialog = AppMessageDialogSingleton.getSingleton();
            PropertiesManager props = PropertiesManager.getPropertiesManager();
            dialog.show(props.getProperty(UPDATE_ERROR_TITLE), props.getProperty(UPDATE_ERROR_MESSAGE));
        }
    }

    public void reloadInfoPane() {
    }

    public void reloadCanvas() {
        viewAreaPane.getChildren().clear();

        //load canvas
        for (DesignObj dObj : DataManager.getDObjList()) {
            try {
                Node child = dObj.getRenderObject();

                boolean add = viewAreaPane.getChildren().add(child);

                if (!add) {
                    System.out.println("Failed to add the object: " + dObj);
                } else{
                    System.out.println("Successfully added the object: " + dObj);
                }

            } catch (IllegalArgumentException iae) {
                System.out.println(iae.getMessage());
            }
        }

//        try {
//            Node child = poly.getRenderObject();
//
//            boolean add = viewAreaPane.getChildren().add(child);
//
//            if (!add) {
//                System.out.println("Failed to add the object");
//            }
//
//        } catch (IllegalArgumentException iae) {
//            System.out.println(iae.getMessage());
//        }
    }

    @Override
    public void updateCustomToolbarControl(boolean workspaceActivated) {
        redoButton.setDisable(!workspaceActivated || DataManager.isRedoLogEmpty());
        undoButton.setDisable(!workspaceActivated || DataManager.isUndoLogEmpty());
    }

    private Group getGrid(double width, double height) {
        Group grid = new Group();
        System.out.println(width + ", " + height);
        for (int i = 0; i < width / 10; i++) {
            grid.getChildren().add(new Line(0, i * 10, width, i * 10));
        }

        for (int i = 0; i < height / 10; i++) {
            grid.getChildren().add(new Line(i * 10, 0, i * 10, height));
        }
        return grid;
    }
}

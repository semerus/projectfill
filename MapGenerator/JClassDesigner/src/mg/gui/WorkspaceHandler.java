/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.gui;

import java.io.File;
import java.io.IOException;
import javafx.beans.property.BooleanProperty;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.embed.swing.SwingFXUtils;
import javafx.scene.Node;
import javafx.scene.SnapshotParameters;
import javafx.scene.image.WritableImage;
import javafx.scene.input.MouseEvent;
import javafx.scene.layout.Pane;
import javafx.scene.paint.Color;
import javafx.scene.shape.Polygon;
import javafx.stage.FileChooser;
import javax.imageio.ImageIO;
import mg.data.DataManager;
import mg.data.obj.DesignObj;
import mg.data.obj.Point;
import mg.data.obj.PolygonObj;
import mg.file.FileManager;
import properties_manager.PropertiesManager;
import static saf.settings.AppPropertyType.LOAD_ERROR_MESSAGE;
import static saf.settings.AppPropertyType.LOAD_ERROR_TITLE;
import static saf.settings.AppPropertyType.SAVE_WORK_TITLE;
import static saf.settings.AppPropertyType.WORK_FILE_EXT;
import static saf.settings.AppPropertyType.WORK_FILE_EXT_DESC;
import static saf.settings.AppStartupConstants.PATH_WORK;
import saf.ui.AppGUI;
import saf.ui.AppMessageDialogSingleton;

/**
 *
 * @author Yun Joon Soh
 */
public class WorkspaceHandler {
    
    private static final double RESIZE_MARGIN = 10.0;
    private static final double SNAP_MARGIN = 10.0;
    static final BooleanProperty SNAP_STATUS = new SimpleBooleanProperty(false);
    static final BooleanProperty GRID_STATUS = new SimpleBooleanProperty(false);
    static final DoubleProperty SCALE_X = new SimpleDoubleProperty();
    static final DoubleProperty SCALE_Y = new SimpleDoubleProperty();
    
    protected static enum REGION {
        TL, T, TR, ML, M, MR, BL, B, BR
    };
    
    private static double orgSceneX, orgSceneY;
    private static double orgTranslateX, orgTranslateY;
    private static REGION currentRegion;
    private static double oriLength, oriHeight;

    /*Handle point creation*/
    static PolygonObj poly = new PolygonObj();
    
    public static void handleMouseClick(MouseEvent m) {
        double X = m.getX();
        double Y = m.getY();
        
        if (SNAP_STATUS.get()) {
            // this is for snapping
            X -= (X % SNAP_MARGIN);
            Y -= (Y % SNAP_MARGIN);
        }
        
        if (m.getTarget().equals(Workspace.viewAreaPane) || m.getTarget() instanceof Polygon) {
            Point p = new Point(X, Y);
            System.out.println("handleMouseClick()" + p + ", event: " + m);
            DataManager.getDObjList().add(p);
            poly.addPoint(p);
        }
        
        DataManager.getApp().getWorkspaceComponent().reloadWorkspace();
    }
    
    public static void zoomin() {
        SCALE_X.set(SCALE_X.get() * 2);
        SCALE_Y.set(SCALE_Y.get() * 2);
    }
    
    public static void zoomout() {
        SCALE_X.set(SCALE_X.get() / 2);
        SCALE_Y.set(SCALE_Y.get() / 2);
    }
    
    public static void handleSelection(MouseEvent e, DesignObj dObj) {
        DataManager.setSelected(dObj);
        DataManager.getSelectedCanvas().set((Node) e.getSource());
    }
    
    public static void handleMouseDraggedDesignObj(MouseEvent t, DesignObj dObj) {
        DataManager.getApp().getGUI().markAsEdited();
        
        double offsetX = t.getSceneX() - orgSceneX;
        double offsetY = t.getSceneY() - orgSceneY;
        double newTranslateX = orgTranslateX + offsetX;
        double newTranslateY = orgTranslateY + offsetY;
        
        if (SNAP_STATUS.get()) {
            // this is for snapping
            newTranslateX -= (newTranslateX % SNAP_MARGIN);
            newTranslateY -= (newTranslateY % SNAP_MARGIN);
        }
        
        dObj.getRenderInfo().posXProperty().set(newTranslateX);
        dObj.getRenderInfo().posYProperty().set(newTranslateY);
        
        DataManager.getApp().getWorkspaceComponent().reloadWorkspace();
    }
    
    public static void handleMousePressed(MouseEvent t, DesignObj dObj) {
        if (t.getClickCount() > 1) {
            return;
        }
        
        if (dObj instanceof Point) {
            System.out.println("handleMousePressed() " + dObj);
            boolean success = DataManager.getDObjList().remove(dObj);
            System.out.println(success ? "Success" : "Failed");
            
            poly.rmPoint((Point) dObj);
        }
        
        orgSceneX = t.getSceneX();
        orgSceneY = t.getSceneY();
        orgTranslateX = ((Node) (t.getSource())).getTranslateX();
        orgTranslateY = ((Node) (t.getSource())).getTranslateY();
    }
    
    public static void handleMouseOver(MouseEvent t, DesignObj dObj, boolean delete) {
        if (delete) {
            dObj.getRenderInfo().colorProperty().set(Color.RED);
        } else {
            dObj.getRenderInfo().colorProperty().set(Color.BLACK);
        }
    }
    
    public static void handleSaveAsRequest(FileManager fm, DataManager dm, AppGUI appGUI) {
        // WE'LL NEED THIS TO GET CUSTOM STUFF
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        try {
            File theDir = new File(PATH_WORK);
            if (!theDir.exists()) {
                System.out.println("creating directory: " + PATH_WORK);
                boolean result = false;
                
                try {
                    theDir.mkdir();
                    result = true;
                } catch (SecurityException se) {
                    //handle it
                }
                if (result) {
                    System.out.println("DIR created");
                }
            }
            // PROMPT THE USER FOR A FILE NAME
            FileChooser fc = new FileChooser();
            fc.setInitialDirectory(theDir);
            fc.setTitle(props.getProperty(SAVE_WORK_TITLE));
            fc.getExtensionFilters().addAll(
                    new FileChooser.ExtensionFilter(props.getProperty(WORK_FILE_EXT_DESC), props.getProperty(WORK_FILE_EXT)));
            
            File selectedFile = fc.showSaveDialog(appGUI.getWindow());
            
            if (selectedFile != null) {
                String extension;
                
                int i = selectedFile.getName().lastIndexOf('.');
                if (i > 0) {
                    extension = selectedFile.getName().substring(i + 1);
                    if (!extension.equals(props.getProperty(WORK_FILE_EXT))) {
                        selectedFile = new File(selectedFile.getAbsolutePath() + "." + props.getProperty(WORK_FILE_EXT));
                    }
                } else {
                    selectedFile = new File(selectedFile.getAbsolutePath() + "." + props.getProperty(WORK_FILE_EXT));
                }
                fm.saveData(dm, selectedFile.getAbsolutePath());
            }
        } catch (IOException ioe) {
            ioe.printStackTrace();
            AppMessageDialogSingleton dialog = AppMessageDialogSingleton.getSingleton();
            dialog.show(props.getProperty(LOAD_ERROR_TITLE), props.getProperty(LOAD_ERROR_MESSAGE));
        }
    }
    
    public static boolean handleSnapshotRequest(Pane canvasPane, AppGUI gui) {
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        FileChooser fc = new FileChooser();
        File initDir = new File(PATH_WORK);
        if (!initDir.exists()) {
            initDir.mkdirs(); //create only non-existent folders
        }
        fc.setInitialDirectory(new File(PATH_WORK));
        fc.getExtensionFilters().addAll(
                new FileChooser.ExtensionFilter("Png File", "*.png"));
        
        File selectedFile = fc.showSaveDialog(gui.getWindow());
        //Code to append .png if no extension exists
        if (selectedFile != null && !selectedFile.getName().contains(".")) {
            selectedFile = new File(selectedFile.getAbsolutePath() + ".png");
        }
        
        try {
            WritableImage wi = new WritableImage((int) canvasPane.getBoundsInLocal().getWidth(), (int) canvasPane.getBoundsInLocal().getHeight());
            WritableImage snapshot = canvasPane.snapshot(new SnapshotParameters(), wi);
            
            ImageIO.write(SwingFXUtils.fromFXImage(snapshot, null), "png", selectedFile);
        } catch (IOException ex) {
        }

//        } catch (IOException ioe) {
//	    AppMessageDialogSingleton dialog = AppMessageDialogSingleton.getSingleton();
//	    dialog.show(props.getProperty(SAVE_ERROR_TITLE), props.getProperty(SAVE_ERROR_MESSAGE));
//        }
        return false;
    }
    
    public static void handleGeneratePolygon() {
        if(poly.getPoints().isEmpty()){
            System.out.println("Polygon is empty");
            return;
        }
        
        PolygonObj created = new PolygonObj();
        for(Point p : poly.getPoints()){
            created.addPoint(p);
        }
        poly.reset();
        
        DataManager.getDObjList().add(created);
    }
}

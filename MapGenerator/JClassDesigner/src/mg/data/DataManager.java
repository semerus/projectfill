package mg.data;

import java.util.ArrayList;
import java.util.Stack;
import javafx.beans.property.BooleanProperty;
import javafx.beans.property.ObjectProperty;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.beans.property.SimpleObjectProperty;
import javafx.scene.Node;
import javax.json.Json;
import javax.json.JsonArray;
import javax.json.JsonArrayBuilder;
import mg.data.obj.DesignObj;
import saf.AppTemplate;
import saf.components.AppDataComponent;

/**
 * This class serves as the data management component for this application.
 *
 * @author Richard McKenna
 * @author Yun Joon Soh
 * @version 1.0
 */
public class DataManager implements AppDataComponent {
    //constants

    // THIS IS A SHARED REFERENCE TO THE APPLICATION
    protected static AppTemplate app;

    protected static ArrayList<DesignObj> dObjList;

    private static Stack<DataModifyLog> undoLog, redoLog;

    static ObjectProperty<DesignObj> selectedDObj;

    static ObjectProperty<Node> selectedCanvas = new SimpleObjectProperty<>();

    public final static BooleanProperty IS_DBOX_SELECTED = new SimpleBooleanProperty();

    ;

    public static boolean isUndoLogEmpty() {
        return undoLog.isEmpty();
    }

    public static boolean isRedoLogEmpty() {
        return redoLog.isEmpty();
    }

    public static void pushToUndoLog(DataModifyLog dataModifyLog) {
        undoLog.push(dataModifyLog);
        System.out.println("Pushed to undoList");
    }

    public static void pushToRedoLog(DataModifyLog dataModifyLog) {
        redoLog.push(dataModifyLog);
    }

    public static DataModifyLog popFromUndoLog() {
        DataModifyLog ret = undoLog.pop();
        redoLog.push(ret);
        return ret;
    }

    public static DataModifyLog popFromRedoLog() {
        DataModifyLog ret = redoLog.pop();
        undoLog.push(ret);
        return ret;
    }

    public static void resetLog() {
        undoLog.clear();
        redoLog.clear();
    }

    /**
     * THis constructor creates the data manager and sets up the
     *
     *
     * @param initApp The application within which this data manager is serving.
     */
    public DataManager(AppTemplate initApp) throws Exception {
        // KEEP THE APP FOR LATER
        app = initApp;

        dObjList = new ArrayList<>();

        undoLog = new Stack<>();
        redoLog = new Stack<>();

        selectedDObj = new SimpleObjectProperty<>();

        selectedCanvas.addListener((obs, oldNode, newNode) -> {
            if (oldNode != null) {
                oldNode.setStyle("");
            }
            if (newNode != null) {
                newNode.setStyle("-fx-effect: innershadow(gaussian, #039ed3, 10, 1.0, 0, 0);");
            }
        }
        );

    }

    @Override
    public void reset() {
        dObjList = new ArrayList<>();

        undoLog = new Stack<>();
        redoLog = new Stack<>();

        selectedCanvas.set(null);
        selectedDObj.set(null);
    }

    public static AppTemplate getApp() {
        return app;
    }

    public static ArrayList<DesignObj> getDObjList() {
        return dObjList;
    }

    public static ObjectProperty<DesignObj> getSelectedDObj() {
        return selectedDObj;
    }

    public static ObjectProperty<Node> getSelectedCanvas() {
        return selectedCanvas;
    }

    public static void setSelected(DesignObj dObj) {
        if (dObj instanceof DesignObj) {
            selectedDObj.set(dObj);
        } else if (dObj == null) {
            selectedDObj.set(null);
        }
    }


    public static JsonArray getDObjListInJson() {
        JsonArrayBuilder arrayBuilder = Json.createArrayBuilder();

        for (DesignObj dObj : dObjList) {
            arrayBuilder.add(dObj.toJsonObject());
        }

        return arrayBuilder.build();
    }

    public static void setdObjList(ArrayList<DesignObj> dObjList) {
        DataManager.dObjList = dObjList;
    }

}

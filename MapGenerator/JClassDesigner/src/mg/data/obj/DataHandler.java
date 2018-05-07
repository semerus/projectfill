/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.obj;

import java.io.File;
import java.io.IOException;
import javafx.stage.DirectoryChooser;
import static mg.PropertyType.EXPORT_TO_CODE_TITLE;
import static mg.PropertyType.TO_CODE_IOERROR_MESSAGE;
import static mg.PropertyType.TO_CODE_IOERROR_TITLE;
import static mg.PropertyType.TO_CODE_SUCCESS_MESSAGE;
import static mg.PropertyType.TO_CODE_SUCCESS_TITLE;
import mg.data.DataManager;
import mg.data.DataModifyLog;
import properties_manager.PropertiesManager;
import saf.ui.AppMessageDialogSingleton;

/**
 *
 * @author YunJoonSoh
 */
public class DataHandler {

    public static void handleUndo() {
        DataManager.getApp().getGUI().markAsEdited();

        DataModifyLog log = DataManager.popFromUndoLog();
        System.out.println("Undo Called: " + log.getLogType().toString());

        switch (log.getLogType()) {
            case DObj_ADD:
                boolean result = DataManager.getDObjList().remove(log.getWhat());
                System.out.println("UNDO REMOVE: " + result);
                break;
            case DOBJ_REMOVE:
                DataManager.getDObjList().add(log.getWhat());
                break;
        }
        DataManager.getApp().getWorkspaceComponent().reloadWorkspace();
    }

    public static void handleRedo() {
        DataManager.getApp().getGUI().markAsEdited();

        DataModifyLog log = DataManager.popFromRedoLog();

        switch (log.getLogType()) {
            case DObj_ADD:
                DataManager.getDObjList().add(log.getWhat());
                break;
            case DOBJ_REMOVE:
                DataManager.getDObjList().remove(log.getWhat());
                break;
        }
        DataManager.getApp().getWorkspaceComponent().reloadWorkspace();
    }

    public static void handleToCode() {
        PropertiesManager props = PropertiesManager.getPropertiesManager();
        DirectoryChooser dc = new DirectoryChooser();
        dc.setTitle(props.getProperty(EXPORT_TO_CODE_TITLE));
        File selectedFile = dc.showDialog(DataManager.getApp().getGUI().getWindow());

        if (selectedFile == null) {
            return;
        }

        try {
            DataManager.getApp().getFileComponent().exportData(null, selectedFile.getAbsolutePath());
        } catch (IOException ex) {
            System.out.println(props.getProperty(TO_CODE_IOERROR_MESSAGE));
            System.out.println(ex.getMessage());
            AppMessageDialogSingleton dialog = AppMessageDialogSingleton.getSingleton();
            dialog.show(props.getProperty(TO_CODE_IOERROR_TITLE), props.getProperty(TO_CODE_IOERROR_MESSAGE));
        }

        AppMessageDialogSingleton dialog = AppMessageDialogSingleton.getSingleton();
        dialog.show(props.getProperty(TO_CODE_SUCCESS_TITLE), props.getProperty(TO_CODE_SUCCESS_MESSAGE) + " " + selectedFile);
    }
}
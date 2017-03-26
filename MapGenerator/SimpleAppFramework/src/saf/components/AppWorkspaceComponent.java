package saf.components;

import javafx.beans.property.BooleanProperty;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.scene.layout.BorderPane;

/**
 * This abstract class provides the structure for workspace components in our
 * applications. Note that by doing so we make it possible for customly provided
 * descendent classes to have their methods called from this framework.
 *
 * @author Richard McKenna
 * @author Yun Joon Soh
 * @version 1.0
 */
public abstract class AppWorkspaceComponent implements AppStyleArbiter {

    // THIS DENOTES THAT THE USER HAS BEGUN WORKING AND
    // SO THE WORKSPACE IS VISIBLE AND USABLE
    protected BooleanProperty workspaceDeactivated = new SimpleBooleanProperty(true);
    
    /**
     * When called this function puts the workspace into the window,
     * revealing the controls for editing work.
     * 
     * @param appPane The pane that contains all the controls in the
     * entire application, including the file toolbar controls, which
     * this framework manages, as well as the customly provided workspace,
     * which would be different for each app.
     */
    public void activateWorkspace(BorderPane appPane) {
        workspaceDeactivated.set(false);
    }
    
    public boolean workspaceActivated() {
        return workspaceDeactivated.get();
    }

    // THE DEFINITION OF THIS CLASS SHOULD BE PROVIDED
    // BY THE CONCRETE WORKSPACE
    public abstract void reloadWorkspace();
    public abstract void updateCustomToolbarControl(boolean workspaceActivated);
}

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg;

/**
 *
 * @author YunJoonSoh
 */
public class EnumContainer {

    public enum LOG_TYPE {
        DObj_ADD, DBOX_ATTR_CHANGE, DOBJ_REMOVE
    }

    public enum LINE_TYPE {
        IMPLEMENTS, EXTENDS
    }


    public static enum DESIGN_BOX_ATTRIBUTE {
        CLASS_NAME, PACKAGE_NAME, PARENT_LIST, VARIABLE, METHOD
    }
}

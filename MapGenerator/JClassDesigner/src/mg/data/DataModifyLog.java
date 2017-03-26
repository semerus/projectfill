/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data;

import mg.EnumContainer.DESIGN_BOX_ATTRIBUTE;
import mg.EnumContainer.LOG_TYPE;
import mg.data.obj.DesignObj;
import static mg.EnumContainer.LOG_TYPE.DBOX_ATTR_CHANGE;

/**
 *
 * @author YunJoonSoh
 */
public class DataModifyLog {

    DesignObj what;
    Object before, after;
    DESIGN_BOX_ATTRIBUTE attr;
    LOG_TYPE logType;

    public DataModifyLog(DesignObj what, LOG_TYPE logType) {
        this.what = what;
        this.logType = logType;
    }

    public DataModifyLog(DesignObj what, DESIGN_BOX_ATTRIBUTE attr, Object before, Object after) {
        this.what = what;
        this.before = before;
        this.after = after;
        this.attr = attr;
        this.logType = DBOX_ATTR_CHANGE;
    }

    public LOG_TYPE getLogType() {
        return logType;
    }

    public DesignObj getWhat() {
        return what;
    }

    public Object getBefore() {
        return before;
    }

    public Object getAfter() {
        return after;
    }

    public DESIGN_BOX_ATTRIBUTE getAttr() {
        return attr;
    }
}

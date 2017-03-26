/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.obj;

import mg.data.info.RenderInfo;
import javafx.scene.Node;
import javax.json.JsonObject;

/**
 *
 * @author YunJoonSoh
 */
public abstract class DesignObj {
    private static int idNum = 0;
    RenderInfo ri;
    int id;
    Node shape;
    
    public DesignObj(){
        id = ++idNum;
    }
    
    public RenderInfo getRenderInfo(){
        return ri;
    }
    
    public void updateRI(RenderInfo ri){
        this.ri = ri;
    }
    
    public abstract Node getRenderObject();
    public abstract JsonObject toJsonObject();
}

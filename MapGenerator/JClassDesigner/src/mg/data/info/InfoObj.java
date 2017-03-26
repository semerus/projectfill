/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.info;

import javax.json.JsonObject;

/**
 *
 * @author YunJoonSoh
 */
public interface InfoObj {
    public String toDisplayString();
    public JsonObject toJsonObject();
    public String toCode();
}

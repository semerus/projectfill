/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.obj;

import java.util.Objects;
import javafx.scene.Node;
import javax.json.JsonObject;
import mg.data.info.RenderInfo;
import mg.file.FileManager;
import mg.gui.PointRender;

public class Point extends DesignObj {

    public static final double DEFAULT_POINT_SIZE = 7;
    LineSeg parentLingSeg;

    public Point() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    public Point(double posX, double posY) {
        this.ri = new RenderInfo(posX, posY, DEFAULT_POINT_SIZE, DEFAULT_POINT_SIZE);
        this.parentLingSeg = null;
    }

    public Point(double posX, double posY, LineSeg parentLineSeg) {
        this.ri = new RenderInfo(posX, posY, DEFAULT_POINT_SIZE, DEFAULT_POINT_SIZE);
        this.parentLingSeg = parentLineSeg;
    }

    public LineSeg getParentLineSeg() {
        return parentLingSeg;
    }

    @Override
    public JsonObject toJsonObject() {
        return FileManager.makeDesignObjJsonObject(this);
    }

    @Override
    public Node getRenderObject() {
        shape = new PointRender(this);
        return shape;
    }

    @Override
    public String toString() {
        StringBuilder s = new StringBuilder();

        s.append("Point(");
        s.append(ri.toString());
        s.append(")");

        return s.toString();
    }

    @Override
    public boolean equals(Object that) {
        if (!(that instanceof Point)) {
            return false;
        }

        Point target = (Point) that;

        if (!ri.equals(target.ri)) {
            return false;
        }

        return true;
    }

    @Override
    public int hashCode() {
        int hash = 3;
        hash = 97 * hash + Objects.hashCode(this.parentLingSeg);
        return hash;
    }
}

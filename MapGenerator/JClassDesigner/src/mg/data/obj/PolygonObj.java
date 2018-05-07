/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.data.obj;

import java.util.ArrayList;
import java.util.List;
import javafx.scene.Node;
import javax.json.JsonObject;
import mg.data.info.RenderInfo;
import mg.file.FileManager;
import mg.gui.PolygonRenderer;

/**
 *
 * @author Mukum
 */
public class PolygonObj extends DesignObj {

    private static final RenderInfo DEF_ri = new RenderInfo(0, 0, 0, 0);
    private List<Point> points;

    public PolygonObj() {
        this.points = new ArrayList<>();
        ri = DEF_ri;
    }

    public void addPoint(Point p) {
        points.add(p);
    }

    public void rmPoint(Point p) {
        points.remove(p);
    }

    public List<Point> getPoints() {
        return points;
    }

    public void reset() {
        this.points = new ArrayList<>();
        ri = DEF_ri;
    }

    @Override
    public Node getRenderObject() {
        List<Double> vertices = new ArrayList<>();

        for (Point p : points) {
            vertices.add(p.getRenderInfo().getPosX());
            vertices.add(p.getRenderInfo().getPosY());
        }

        return new PolygonRenderer(vertices);
    }

    @Override
    public JsonObject toJsonObject() {
        return FileManager.makeDesignObjJsonObject(this);
    }

}

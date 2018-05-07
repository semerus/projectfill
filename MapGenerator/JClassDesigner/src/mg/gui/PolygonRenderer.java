/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.gui;

import java.util.List;
import javafx.scene.paint.Color;
import javafx.scene.shape.Polygon;

/**
 *
 * @author Mukum
 */
public class PolygonRenderer extends Polygon{
    public PolygonRenderer(List<Double> vertices){
        getPoints().addAll(vertices);
        setFill(Color.TRANSPARENT);
        setStroke(Color.BLUE);
    }
}

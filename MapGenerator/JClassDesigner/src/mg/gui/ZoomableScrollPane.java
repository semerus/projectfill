/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.gui;

import javafx.scene.Group;
import javafx.scene.Node;
import javafx.scene.control.ScrollPane;
import javafx.scene.transform.Scale;

/**
 *
 * @author https://pixelduke.wordpress.com/2012/09/16/zooming-inside-a-scrollpane/
 */
public class ZoomableScrollPane extends ScrollPane {

    Group zoomGroup;
    Scale scaleTransform;
    Node content;

    public ZoomableScrollPane(Node content) {
        this.content = content;
        Group contentGroup = new Group();
        zoomGroup = new Group();
        contentGroup.getChildren().add(zoomGroup);
        zoomGroup.getChildren().add(content);
        setContent(contentGroup);
        scaleTransform = new Scale(zoomGroup.getScaleX(), zoomGroup.getScaleY(), 0, 0);
        zoomGroup.getTransforms().add(scaleTransform);
    }
}

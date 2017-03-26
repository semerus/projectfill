/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package mg.test_bed;

import mg.JClassDesigner;
import mg.data.DataManager;

import mg.data.obj.DesignObj;
import mg.file.FileManager;
import saf.components.AppComponentsBuilder;
import saf.components.AppDataComponent;
import saf.components.AppFileComponent;
import static saf.settings.AppStartupConstants.PATH_WORK;

/**
 *
 * @author ysoh
 */
public class TestLoad {

    static AppDataComponent dataComponent;
    static AppFileComponent fileComponent;

    public static void main(String[] args) throws Exception {
        System.out.println("TestLoad Driver started");

        JClassDesigner jcd = new JClassDesigner();
        AppComponentsBuilder builder;
        builder = jcd.makeAppBuilderHook();
        fileComponent = builder.buildFileComponent();
        dataComponent = builder.buildDataComponent();
        DataManager dm = (DataManager) dataComponent;
        FileManager fm = (FileManager) fileComponent;

        fm.loadData(dm, PATH_WORK + "\\DesignSaveTest.json");


        System.out.println("TestLoad Driver Done");
    }
}

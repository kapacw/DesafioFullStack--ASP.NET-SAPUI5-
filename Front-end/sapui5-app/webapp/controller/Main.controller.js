sap.ui.define([
  "sap/ui/core/mvc/Controller",
  "sap/ui/core/mvc/XMLView"
], function (Controller, XMLView) {
  "use strict";

  return Controller.extend("tarefas.front.controller.Main", {

    onInit: function () {
      this._loadTaskView();
    },

    _loadTaskView: function () {
      const oVBox = this.byId("contentVBox");
      if (!oVBox) {
        console.error("VBox contentVBox não encontrado na view Main");
        return;
      }

      oVBox.removeAllItems();

      XMLView.create({
        viewName: "tarefas.front.view.Task"
      }).then(function (oView) {
        oVBox.addItem(oView);
      });
    }
  });
});
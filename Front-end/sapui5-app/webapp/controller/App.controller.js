sap.ui.define([
  "./BaseController"
], function (BaseController) {
  "use strict";

  return BaseController.extend("tarefas.front.controller.App", {
    onInit: function () {
      // Aplica o modo de densidade (compact/cozy), se possível
      const oComponent = this.getOwnerComponent();
      if (oComponent && oComponent.getContentDensityClass) {
        this.getView().addStyleClass(oComponent.getContentDensityClass());
      }

      // Inicializa o roteador
      if (oComponent && oComponent.getRouter) {
        oComponent.getRouter().initialize();
      }
    }
  });
});

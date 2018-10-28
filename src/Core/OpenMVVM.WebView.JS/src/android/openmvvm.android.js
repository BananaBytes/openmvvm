import { viewLoader } from "./ViewLoader";
import { bridge } from "./AndroidBridge";
import ParserService from "../ParserService";
import RendererService from "../RendererService";
import BridgeMapper from "../BridgeMapper";
import OpenMvvmService from "../OpenMvvmService";

const context = {
    elementCache: {},
    collectionCache: {}
};

const mapper = new BridgeMapper(bridge);
const parserService = new ParserService(mapper, context);
const rendererService = new RendererService(parserService, context);
const openMvvm = new OpenMvvmService(mapper, viewLoader, parserService, rendererService, context);

bridge.start(openMvvm);
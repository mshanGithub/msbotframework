# Prebuilt Language Models

Prebuilt language models have been trained towards more sophisticated tasks for both monolingual as well as multilingual scenarios,
including intent prediction and entity extraction.

## Models Description
The public preview of Orchestrator includes the following prebuilt language models available in [versions repository][2].

### pretrained.20200924.microsoft.dte.00.03.en.onnx
This is a fast and small EN-only base model for intent detection with sufficient prediction performance.
We suggest using this model if speed and memory size is critical to your deployment environment,
otherwise consider other options. It is a generic 3-layer pretrained
[Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20200924.microsoft.dte.00.06.en.onnx
This is a high quality EN-only base model for intent detection that strikes the balance between size,
speed and predictive performance.
It is a 6-layer pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]),
thus it can be used out of box. This is the default model used if none explicitly specified.

### pretrained.20200924.microsoft.dte.00.12.en.onnx
This is a high quality EN-only base model for intent detection, but is larger and slower than other options.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210205.microsoft.dte.00.12.bert_example_ner.en.onnx (preview)
This is a high quality EN-only base model for entity extraction.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20201210.microsoft.dte.00.12.unicoder_multilingual.onnx
This is a high quality multilingual base model for intent detection.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210105.microsoft.dte.00.12.bert_example_ner_multilingual.onnx (preview)
This is a high quality multilingual base model for entity extraction.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210105.microsoft.dte.00.12.tulr_example_ner_multilingual.onnx (preview)
This is a high quality multilingual base model for entity extraction.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210205.microsoft.dte.00.06.bert_example_ner.en.onnx (preview)
This is a high quality EN-only base model for entity extraction. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20201210.microsoft.dte.00.06.unicoder_multilingual.onnx
This is a high quality multilingual base model for intent detection. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210205.microsoft.dte.00.06.bert_example_ner_multilingual.onnx (preview)
This is a high quality multilingual base model for entity extraction. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210205.microsoft.dte.00.06.tulr_example_ner_multilingual.onnx (preview)
This is a high quality multilingual base model for entity extraction. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

## Models Evaluation
For a more quantitative comparison analysis of the different models see the following performance characteristics.

### Model attributes

The following table shows the size & speed performance attributes.

|  Model |Base Model   |Layers  |Encoding time per query | Disk Allocation |
| ------------ | ------------ | ------------ | ------------ | ------------ |
|pretrained.20200924.microsoft.dte.00.03.en.onnx |   BERT | 3  |  ~ 7 ms |  164M |
|pretrained.20200924.microsoft.dte.00.06.en.onnx | BERT | 6  |  ~ 16 ms | 261M  |
|pretrained.20200924.microsoft.dte.00.12.en.onnx | BERT    | 12  | ~ 26 ms  | 427M  |

### Model performance, evaluated by micro-average-accuracy

The following table shows how accurate is each model relative to provided training sample size using [Snips NLU][4] system.

|Training samples per intent   |5   |10   |25   |50   |100   |200   |
| ------------ | ------------ | ------------ | ------------ | ------------ | ------------ |------------ |
|pretrained.20200924.microsoft.dte.00.03.en.onnx |  0.756  | 0.839  | 0.904  | 0.929  | 0.943  | 0.951  |
|pretrained.20200924.microsoft.dte.00.06.en.onnx |   0.924 | 0.940  | 0.957  |  0.960 |  0.966 | 0.969  |
|pretrained.20200924.microsoft.dte.00.12.en.onnx |  0.902  |  0.931 |  0.951 | 0.960  |  0.964 |  0.969 |


## License

The models are released under the following [License Terms][6].

## References

* [UniLMv2 Paper][1]

* [Base Models Versions Repository][2]

* [KNN (K nearest neighbors algorithm)][3]

* [Snips NLU (Natural Language Understanding)][4]

* [Snips NLU Metrics][5]

* [Transformer][7]

[1]: https://arxiv.org/abs/2002.12804 "UniLMv2: Pseudo-Masked Language Models for Unified Language Model Pre-Training"
[2]: https://aka.ms/nlrversions
[3]: https://en.wikipedia.org/wiki/K-nearest_neighbors_algorithm
[4]: https://github.com/snipsco/snips-nlu "Snips NLU"
[5]: https://github.com/snipsco/snips-nlu-metrics "Snips NLU Metrics"
[6]: ./LICENSE.md "License agreement"
[7]: https://en.wikipedia.org/wiki/Transformer_(machine_learning_model)

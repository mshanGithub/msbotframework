# Prebuilt Language Models

Prebuilt language models have been trained towards more sophisticated tasks for both monolingual as well as multilingual scenarios, including intent prediction and entity extraction.
Entity extraction is currently experimental and not yet readt for production use.

The following prebuilt language models are now available in [versions repository][2].

See the [References](#references) section for technical descriptions of the AI technology behind the models.

## Default Models

### pretrained.20200924.microsoft.dte.00.06.en.onnx
This is a high quality EN-only base model for intent detection that strikes the balance between size,
speed and predictive performance.
It is a 6-layer pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]),
thus it can be used out of box. This is the default model used if none explicitly specified.

### pretrained.20210205.microsoft.dte.00.06.unicoder_multilingual.onnx
This is a high quality multilingual base model for intent detection. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box. The model supports in total 100 languages (full list can be found at [XLMR Supported Languages][8]). 8 languages (EN, ES, DE, FR, IT, JA, PT, and ZH) are fine-tuned with additional data (performance can be found [here](#multilingual-intent-detection-models-evaluation)). 

## Alternate Models

### pretrained.20200924.microsoft.dte.00.03.en.onnx
This is a fast and small EN-only base model for intent detection with sufficient prediction performance.
We suggest using this model if speed and memory size is critical to your deployment environment,
otherwise consider other options. It is a generic 3-layer pretrained
[Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20200924.microsoft.dte.00.12.en.onnx
This is a high quality EN-only base model for intent detection, but is larger and slower than other options.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20201210.microsoft.dte.00.12.unicoder_multilingual.onnx
This is a high quality multilingual base model for intent detection.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box. The model supports in total 100 languages (full list can be found at [XLMR Supported Languages][8]). 8 languages (EN, ES, DE, FR, IT, JA, PT, and ZH) are fine-tuned with additional data (performance can be found [here](#multilingual-intent-detection-models-evaluation)). 

## Experimental Models

### pretrained.20210205.microsoft.dte.00.12.bert_example_ner.en.onnx (experimental)
This is a high quality EN-only base model for entity extraction.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210218.microsoft.dte.00.12.bert_example_ner.en.onnx (experimental)
This is a yet another high quality EN-only base model for entity extraction.
It is a 12-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210205.microsoft.dte.00.06.bert_example_ner.en.onnx (experimental)
This is a high quality EN-only base model for entity extraction. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

### pretrained.20210218.microsoft.dte.00.06.bert_example_ner.en.onnx (experimental)
This is a high quality EN-only base model for entity extraction. It's smaller and faster than its 12-layer alternative.
It is a 6-layer pretrained pretrained [Transformer][7] model optimized for conversation.
Its architecture is pretrained for example-based use ([KNN][3]), thus it can be used out of box.

## Models Evaluation
For a more quantitative comparison analysis of the different models see the following performance characteristics.

### English Intent Detection Models Evaluation

- The following table shows the size & speed performance attributes.


|  Model |Base Model   |Layers  |Encoding time per query | Disk Allocation |
| ------------ | ------------ | ------------ | ------------ | ------------ |
|pretrained.20200924.microsoft.dte.00.03.en.onnx |   BERT | 3  |  ~ 7 ms |  164M |
|pretrained.20200924.microsoft.dte.00.06.en.onnx | BERT | 6  |  ~ 16 ms | 261M  |
|pretrained.20200924.microsoft.dte.00.12.en.onnx | BERT    | 12  | ~ 26 ms  | 427M  |

- 
  The following table shows how accurate is each model relative to provided training sample size using [Snips NLU][4] system, evaluated by **micro-average-accuracy**.


|Training samples per intent   |5   |10   |25   |50   |100   |200   |
| ------------ | ------------ | ------------ | ------------ | ------------ | ------------ |------------ |
|pretrained.20200924.microsoft.dte.00.03.en.onnx |  0.756  | 0.839  | 0.904  | 0.929  | 0.943  | 0.951  |
|pretrained.20200924.microsoft.dte.00.06.en.onnx |   0.924 | 0.940  | 0.957  |  0.960 |  0.966 | 0.969  |
|pretrained.20200924.microsoft.dte.00.12.en.onnx |  0.902  |  0.931 |  0.951 | 0.960  |  0.964 |  0.969 |

### Multilingual Intent Detection Models Evaluation
- The following table shows the size & speed performance attributes.

| Model                                                        | Base Model | Layers | Encoding time per query | Disk Allocation |
| ------------------------------------------------------------ | ---------- | ------ | ----------------------- | --------------- |
| pretrained.20210205.microsoft.dte.00.06.unicoder_multilingual.onnx | Unicoder   | 6      | ~ 16 ms                 | 896M            |
| pretrained.20201210.microsoft.dte.00.12.unicoder_multilingual.onnx | Unicoder   | 12     | ~ 30 ms                 | 1.08G           |

- The following table shows how accurate is each model by training and testing on the same language, evaluated by **micro-average-accuracy** on an internal dataset.

| Model                                                        | de-de | en-us | es-es | es-mx | fr-ca | fr-fr | it-it | ja-jp | pt-br | zh-cn |
| ------------------------------------------------------------ | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- |
| pretrained.20210205.microsoft.dte.00.06.unicoder_multilingual.onnx | 0.638 | 0.785 | 0.662 | 0.760 | 0.723 | 0.661 | 0.701 | 0.786 | 0.735 | 0.805 |
| pretrained.20201210.microsoft.dte.00.12.unicoder_multilingual.onnx | 0.642 | 0.764 | 0.646 | 0.754 | 0.722 | 0.636 | 0.689 | 0.789 | 0.725 | 0.809 |

- The following table shows how accurate is each model by training on **en-us** and testing on the different languages, evaluated by **micro-average-accuracy** on an internal dataset.

| Model                                                        | de-de | en-us | es-es | es-mx | fr-ca | fr-fr | it-it | ja-jp | pt-br | zh-cn |
| ------------------------------------------------------------ | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- | ----- |
| pretrained.20210205.microsoft.dte.00.06.unicoder_multilingual.onnx | 0.495 | 0.785 | 0.530 | 0.621 | 0.560 | 0.518 | 0.546 | 0.663 | 0.568 | 0.687 |
| pretrained.20201210.microsoft.dte.00.12.unicoder_multilingual.onnx | 0.499 | 0.764 | 0.529 | 0.604 | 0.562 | 0.515 | 0.547 | 0.646 | 0.555 | 0.681 |

### English Entity Extraction Models Evaluation

- The following table shows the size & speed performance attributes.

| Model                                                        | Base Model | Layers | Encoding time per query | Disk Allocation |
| ------------------------------------------------------------ | ---------- | ------ | ----------------------- | --------------- |
| pretrained.20210205.microsoft.dte.00.06.bert_example_ner.en.onnx | BERT       | 6      | ~ 23 ms                 | 259M            |
| pretrained.20210205.microsoft.dte.00.12.bert_example_ner.en.onnx | BERT       | 12     | ~ 40 ms                 | 427M            |
| pretrained.20210218.microsoft.dte.00.06.bert_example_ner.en.onnx | BERT       | 6      | ~ 23 ms                 | 259M            |
| pretrained.20210218.microsoft.dte.00.12.bert_example_ner.en.onnx | BERT       | 12     | ~ 40 ms                 | 425M            |

- The following table shows how accurate is each model relative to provided training sample size using [Snips NLU][4] system, evaluated by **macro-average-F1**.

| Training samples per entity type                             | 10    | 20    | 50    | 100   | 200   |
| ------------------------------------------------------------ | ----- | ----- | ----- | ----- | ----- |
| pretrained.20210205.microsoft.dte.00.06.bert_example_ner.en.onnx | 0.615 | 0.636 | 0.647 | 0.661 | 0.665 |
| pretrained.20210205.microsoft.dte.00.12.bert_example_ner.en.onnx | 0.637 | 0.658 | 0.684 | 0.698 | 0.702 |
| pretrained.20210218.microsoft.dte.00.06.bert_example_ner.en.onnx | 0.637 | 0.658 | 0.673 | 0.686 | 0.684 |
| pretrained.20210218.microsoft.dte.00.12.bert_example_ner.en.onnx | 0.661 | 0.664 | 0.670 | 0.685 | 0.681 |



## License

The models are released under the following [License Terms][6].

## References

* [UniLMv2 Paper][1]
* [Base Models Versions Repository][2]
* [KNN (K nearest neighbors algorithm)][3]
* [Snips NLU (Natural Language Understanding)][4]
* [Snips NLU Metrics][5]
* [Transformer][7]
* [XLMR Supported Languages][8]
* [Turing Universal Language Representation model (T-ULRv2)][11]
* [Microsoft DeBERTa - Blog][9]
* [Microsoft  AI At Scale][10]

[1]: https://arxiv.org/abs/2002.12804 "UniLMv2: Pseudo-Masked Language Models for Unified Language Model Pre-Training"
[2]: https://aka.ms/nlrversions_0.2
[3]: https://en.wikipedia.org/wiki/K-nearest_neighbors_algorithm
[4]: https://github.com/snipsco/snips-nlu "Snips NLU"
[5]: https://github.com/snipsco/snips-nlu-metrics "Snips NLU Metrics"
[6]: ./LICENSE.md "License agreement"
[7]: https://en.wikipedia.org/wiki/Transformer_(machine_learning_model)
[8]: https://github.com/pytorch/fairseq/tree/master/examples/xlmr#introduction
[9]: https://www.microsoft.com/en-us/research/blog/microsoft-deberta-surpasses-human-performance-on-the-superglue-benchmark/ "Microsoft DeBERTa - Microsoft Blog"
[10]: https://innovation.microsoft.com/en-us/exploring-ai-at-scale "AI at Scale"
[11]: https://www.microsoft.com/en-us/research/blog/microsoft-turing-universal-language-representation-model-t-ulrv2-tops-xtreme-leaderboard/ "Turing Universal Language Representation model"




using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Amg_ingressos_aqui_eventos_api.Repository.Querys
{
    public static class QuerysMongo
    {
        public const string GetEventQuery = @"{
                                $lookup: {
                                    from: 'variants',
                                    'let': { eventId : { '$toString': '$_id' }},
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [
                                                        '$IdEvent',
                                                        '$$eventId'
                                                    ]
                                                }
                                            }
                                        },
                                        {
                                            $lookup: {
                                                from: 'lots',
                                                'let': { variantId : { '$toString': '$_id' }},
                                                pipeline: [
                                                    {
                                                        $match: {
                                                            $expr: {
                                                                $eq: [
                                                                    '$IdVariant',
                                                                    '$$variantId'
                                                                ]
                                                            }
                                                        }
                                                    }
                                                ],
                                                as: 'Lot'
                                            }
                                        },
                                    ],
                                    as: 'Variant'
                                }
                            }";
        public const string GetEventByIdQuery = @"{{$addFields:{{'IdVariantString': {{ '$toString': '$_id' }}}}}},
                            {{ $match: {{ '$and': [{{ 'IdVariantString': '{0}' }}] }}}},
                            {{
                            $lookup: {{
                                from: 'variants',
                                'let': {{ eventId : {{ '$toString': '$_id' }}}},
                                pipeline: [
                                    {{
                                        $match: {{
                                            $expr: {{
                                                $eq: [
                                                    '$IdEvent',
                                                    '$$eventId'
                                                ]
                                            }}
                                        }}
                                    }},
                                    {{
                                        $lookup: {{
                                            from: 'lots',
                                            'let': {{ variantId : {{ '$toString': '$_id' }}}},
                                            pipeline: [
                                                {{
                                                    $match: {{
                                                        $expr: {{
                                                            $eq: [
                                                                '$IdVariant',
                                                                '$$variantId'
                                                            ]
                                                        }}
                                                    }}
                                                }}
                                            ],
                                            as: 'lots'
                                        }}
                                    }},
                                ],
                                as: 'Variants'
                            }}
                        }}";
    }
}
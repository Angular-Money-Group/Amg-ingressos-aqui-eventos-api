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
        public const string GetTicketsByLot = @"{
                                $lookup: {
                                    from: 'tickets',
                                    'let': { lotId : { '$toString': '$_id' }},
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [
                                                        '$IdLot',
                                                        '$$lotId'
                                                    ]
                                                }
                                            }
                                        },
                                        
                                    ],
                                    as: 'tickets'
                                }
                            }";
        public const string GetLotsByVariant = @"{
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
                            }";
        public const string GetEventWithName = @"{
                                        $lookup: {
                                        from: 'producers',
                                                                            'let': { idOrganizer : { '$toString': '$IdOrganizer' }},
                                        pipeline: [
                                                                                {
                                        $match: {
                                        $expr: {
                                        $eq: [{ '$toString': '$_id' },'$$idOrganizer']
                                                                                        }
                                                                                    }
                                                                                },
                                                                                            {
                                                        $project: {
                                                            _id: 0,
                                                            name: 1
                                                        }
                                                    }
                                                                            ],
                                        as: 'User'
                                }
                            }";
        public const string GetTicketByIdDataUser = @"{
                                $lookup: {
                                    from: 'customers',
                                    let: { idUser: { $toString: '$IdUser' } },
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [{ $toString: '$_id' }, '$$idUser']
                                                }
                                            }
                                        }
                                    ],
                                    as: 'User'
                                }
                            }";
        public const string GetTicketByIdDataLot = @"{
                                $lookup: {
                                    from: 'lots',
                                    let: { idLot: { $toString: '$IdLot' } },
                                    pipeline: [
                                        {
                                            $match: {
                                                $expr: {
                                                    $eq: [{ $toString: '$_id' }, '$$idLot']
                                                }
                                            }
                                        }
                                    ],
                                    as: 'Lot'
                                }
                            }";

    
    }
}